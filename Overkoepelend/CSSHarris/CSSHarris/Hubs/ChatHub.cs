using Microsoft.AspNetCore.SignalR;
using CSSHarris.Models.ChatModels;
using ChatUser = CSSHarris.Models.ChatUser;
using Microsoft.AspNetCore.Authorization;
using System.Security.Principal;
using System.Security.Claims;
using CSSHarris.Data;
using CSSHarris.Models;
using System.Collections.Concurrent;

namespace CSSHarris.Hubs
{
    [AllowAnonymous]
    public class ChatHub : Hub<IConnectionHub>
    {
        private readonly ApplicationDbContext db;
        private static ConcurrentDictionary<string, bool> CancelDict = new();

        public ChatHub(ApplicationDbContext db)
        {
            this.db = db;
        }

        public async Task SendMessage(string roomID, string message)
        {
            ChatUser signallingUser = db.ChatUsers.Where(item => item.ConnectionId == Context.ConnectionId).FirstOrDefault();
            Room room = db.Rooms.Where(r => r.ID == roomID).FirstOrDefault();

            Message newmessage = new Message()
            {
                Username = signallingUser.UserName,
                DateTime = DateTime.Now,
                Content = message
            };
            room.Chatlog.Messages.Add(newmessage);

            db.Add(newmessage);
            db.Update(room.Chatlog);
            db.Update(room);
            db.SaveChanges();

            await Clients.Group(roomID).ReceiveMessage(signallingUser.UserName, message);
        }

        public async Task Join(string username)
        {
            IIdentity currentUser = Context.User.Identity;

            var identity = (ClaimsIdentity)Context.User.Identity;
            var userIdClaim = identity.FindFirst(ClaimTypes.NameIdentifier);
            var userId = (userIdClaim is not null) ? userIdClaim.Value : null;

            ChatUser user = db.ChatUsers.Where(user => user.UserID == userId).FirstOrDefault();
            if (user is null)
            {
                Guid guid = Guid.NewGuid();
                user = new()
                {
                    ID = guid.ToString(),
                    UserID = userId,
                    UserName = currentUser.IsAuthenticated ? currentUser.Name : username,
                    ConnectionId = Context.ConnectionId
                };

                db.ChatUsers.Add(user);
            } else
            {
                user.ConnectionId = Context.ConnectionId;
                db.ChatUsers.Update(user);
            }
            db.SaveChanges();
            
            //Get rooms
            await Clients.Caller.UpdateRoomList(db.Rooms.ToList());
            await Clients.Caller.Connected(user.UserName);
        }

        public async Task CreateRoom(string title)
        {
            // Create room
            if (!Context.User.Identity.IsAuthenticated) return;
            if (db.Rooms.Where(room => room.Owner == Context.User.Identity.Name).ToList().Count >= 3) return;
            Guid guid = Guid.NewGuid();
            Room room = new()
            {
                Owner = Context.User.Identity.Name,
                Title = title,
                ID = guid.ToString()
            };

            db.Add(room);
            db.SaveChanges();

            // Send down the new list to all clients
            await SendRoomListUpdate();
        }

        public async Task DeleteRoom(string roomID)
        {
            var roomToDelete = db.Rooms.SingleOrDefault(u => u.ID == roomID);

            if (Context.User.Identity.Name != roomToDelete.Owner) return;

            await Clients.Group(roomID).UpdateUserList(null);
            await Clients.Group(roomID).RoomDeleted();

            List<ChatUser> users = db.ChatUsers.Where(user => user.CurrentRoom == roomToDelete).ToList();

            foreach (ChatUser user in users)
            {
                user.CurrentRoom = null;
                db.Update(user);
                Groups.RemoveFromGroupAsync(user.ConnectionId, roomID);
            }

            db.Remove(roomToDelete);
            db.SaveChanges();

            // Send down the new list to all clients
            await SendRoomListUpdate();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var callingUser = db.ChatUsers.SingleOrDefault(u => u.ConnectionId == Context.ConnectionId);

            if (callingUser == null) return;

            var roomToLeave = callingUser.CurrentRoom;

            if (roomToLeave is not null)
            {
                await RemoveFromGroup(roomToLeave.ID);
                await Clients.Group(roomToLeave.ID).UpdateUserList(db.ChatUsers.Where(user => user.CurrentRoom == roomToLeave).ToList());
            }

            if (callingUser.UserID is not null)
            {
                callingUser.CurrentRoom = null;
                db.Update(callingUser);
            }
            else
            {
                db.ChatUsers.Remove(callingUser);
            }

            db.SaveChanges();
            await base.OnDisconnectedAsync(exception);
        }

        private async Task SendRoomListUpdate()
        {
            await Clients.All.UpdateRoomList(db.Rooms.ToList());
        }

        public async Task JoinRoom(string RoomID)
        {
            var callingUser = db.ChatUsers.SingleOrDefault(u => u.ConnectionId == Context.ConnectionId);
            var roomToJoin = db.Rooms.SingleOrDefault(u => u.ID == RoomID);

            bool IsOwner = roomToJoin.Owner == Context.User.Identity.Name;

            //Join room
            callingUser.CurrentRoom = roomToJoin;
            db.Update(callingUser);
            db.SaveChanges();

            await AddToGroup(RoomID);
            await Clients.Group(RoomID).UpdateUserList(db.ChatUsers.Where(user => user.CurrentRoom == roomToJoin).ToList());

            //TODO do not send full user data
            await Clients.Caller.RoomJoined(roomToJoin.Title, IsOwner, roomToJoin.Chatlog.Messages);
            await Clients.Caller.UpdateRoomList(db.Rooms.ToList());
        }

        public async Task LeaveRoom()
        {
            var callingUser = db.ChatUsers.SingleOrDefault(u => u.ConnectionId == Context.ConnectionId);

            if (callingUser == null) return;

            //Join room
            var roomToLeave = callingUser.CurrentRoom;
            await RemoveFromGroup(roomToLeave.ID);

            callingUser.CurrentRoom = null;

            db.Update(callingUser);
            db.SaveChanges();

            await Clients.Group(roomToLeave.ID).UpdateUserList(db.ChatUsers.Where(user => user.CurrentRoom == roomToLeave).ToList());
            await Clients.Caller.UpdateUserList(null);
            await Clients.Caller.UpdateRoomList(db.Rooms.ToList());
        }

        public async Task AddToGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }

        public async Task RemoveFromGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        }

        public async Task SendFriendRequest(string TargetConnectionId)
        {
            var callingUser = db.ChatUsers.SingleOrDefault(u => u.ConnectionId == Context.ConnectionId);
            var target = db.ChatUsers.Where(item => item.ConnectionId == TargetConnectionId).FirstOrDefault();
            if (target == null || callingUser == null || target.FriendRequests.Contains(callingUser) || callingUser.Friends.Contains(target)) return;

            target.FriendRequests.Add(callingUser);
            db.Update(target);
            db.SaveChanges();
        }
    }
}