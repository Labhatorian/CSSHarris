using Microsoft.AspNetCore.SignalR;
using CSSHarris.Models.ChatModels;
using ChatUser = CSSHarris.Models.ChatUser;
using Microsoft.AspNetCore.Authorization;
using System.Security.Principal;
using System.Security.Claims;
using CSSHarris.Data;
using CSSHarris.Models;
using System.Collections.Concurrent;
using Mailjet.Client.Resources;
using Microsoft.EntityFrameworkCore;
using Message = CSSHarris.Models.ChatModels.Message;
using Microsoft.AspNetCore.Identity;

namespace CSSHarris.Hubs
{
    [AllowAnonymous]
    public class ChatHub : Hub<IConnectionHub>
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> _userManager;

        public ChatHub(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            this.db = db;
            this._userManager = userManager;
        }

        public async Task SendMessage(string roomID, string message)
        {
            var user = await _userManager.GetUserAsync(Context.User);
            if (user is not null && user.Banned) return;

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

            await Clients.Group(roomID).ReceiveMessage(newmessage.ID, signallingUser.UserName, message);
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
                    ChatUserID = guid.ToString(),
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
            var user = await _userManager.GetUserAsync(Context.User);
            if (user is not null && user.Banned) return;

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

        [Authorize(Policy = "RequireModRole")]
        public async Task DeleteMessage(string roomID, string messageID)
        {
            Chatlog chatlog = db.Rooms.SingleOrDefault(u =>  u.ID == roomID).Chatlog;
            Message messageToDelete = chatlog.Messages.SingleOrDefault(u => u.ID == int.Parse(messageID));

            chatlog.Messages.Remove(messageToDelete);
            db.Update(chatlog);
            db.Remove(messageToDelete);
            db.SaveChanges(true);

            await Clients.Group(roomID).ShowMessages(chatlog.Messages);
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var callingUser = db.ChatUsers.SingleOrDefault(u => u.ConnectionId == Context.ConnectionId);

            if (callingUser == null) return;

            var roomToLeave = callingUser.CurrentRoom;

            if (roomToLeave is not null)
            {
                callingUser.CurrentRoom = null;
                db.Update(callingUser);
                await RemoveFromGroup(roomToLeave.ID);
                await Clients.Group(roomToLeave.ID).UpdateUserList(db.ChatUsers.Where(user => user.CurrentRoom == roomToLeave).ToList());
            }

            if (callingUser.UserID is null) db.ChatUsers.Remove(callingUser);

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
            await Clients.Caller.RoomJoined(roomToJoin.Title, IsOwner); 
            await Clients.Caller.ShowMessages(roomToJoin.Chatlog.Messages);
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
    }
}