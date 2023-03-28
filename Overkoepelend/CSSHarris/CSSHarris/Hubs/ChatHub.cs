using CSSHarris.Data;
using CSSHarris.Models;
using CSSHarris.Models.ChatModels;
using Ganss.Xss;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using System.Security.Principal;
using ChatUser = CSSHarris.Models.ChatUser;
using Message = CSSHarris.Models.ChatModels.Message;

namespace CSSHarris.Hubs
{
    //TODO TESTING
    [AllowAnonymous]
    public class ChatHub : Hub<IConnectionHub>
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> _userManager;
        private HtmlSanitizer sanitizer = new HtmlSanitizer();

        public ChatHub(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            this.db = db;
            this._userManager = userManager;
        }

        /// <summary>
        /// Send message to everyone in the room
        /// </summary>
        /// <param name="roomID"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task SendMessage(string roomID, string message)
        {
            ApplicationUser user = null;
            if (_userManager is not null) user = await _userManager.GetUserAsync(Context.User);
            if (user is not null && user.Banned) return;

            ChatUser signallingUser = db?.ChatUsers?.Where(item => item.ConnectionId == Context.ConnectionId).FirstOrDefault();
            Room room = db?.Rooms?.Where(r => r.ID == roomID).FirstOrDefault();

            message = sanitizer.Sanitize(message);

            Message newmessage = new()
            {
                Username = signallingUser.UserName,
                Content = message
            };
            room?.Chatlog.Messages.Add(newmessage);

            if (db is not null || room is not null)
            {
                db.Add(newmessage);
                db.Update(room.Chatlog);
                db.Update(room);
                db.SaveChanges();
            }

            await Clients.Group(roomID).ReceiveMessage(newmessage.ID, signallingUser.UserName, message);
        }

        /// <summary>
        /// SignalR join function, creates a new <see cref="ChatUser"/>
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public async Task Join(string username)
        {
            if (Context.User is not null)
            {
                IIdentity currentUser = Context.User.Identity;

                var identity = (ClaimsIdentity)Context.User.Identity;
                var userIdClaim = identity.FindFirst(ClaimTypes.NameIdentifier);
                var userId = userIdClaim?.Value;
                username = sanitizer.Sanitize(username);

                ChatUser user = db.ChatUsers.Where(user => user.UserID == userId).FirstOrDefault();
                if (user is null)
                {
                    if (db.ChatUsers.Where(user => user.UserName == username) is not null)
                    {
                        Random random = new Random();
                        int randomNumber = random.Next(1, 10000);
                        username = "User " + randomNumber;
                    }

                    Guid guid = Guid.NewGuid();
                    user = new()
                    {
                        ChatUserID = guid.ToString(),
                        UserID = userId,
                        UserName = currentUser.IsAuthenticated ? currentUser.Name : username,
                        ConnectionId = Context.ConnectionId
                    };

                    db.ChatUsers.Add(user);
                }
                else
                {
                    user.ConnectionId = Context.ConnectionId;
                    db.ChatUsers.Update(user);
                }
                db.SaveChanges();

                //Get rooms
                await Clients.Caller.UpdateRoomList(db.Rooms.ToList());
                await Clients.Caller.Connected(user.UserName);
            }
        }

        /// <summary>
        /// Creates a room if a user is logged in, max 3
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public async Task CreateRoom(string title)
        {
            ApplicationUser user = null;
            if (_userManager is not null) user = await _userManager.GetUserAsync(Context.User);
            if (user is not null && user.Banned) return;
            title = sanitizer.Sanitize(title);

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

        /// <summary>
        /// Deletes a room
        /// </summary>
        /// <param name="roomID"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Deletes a message if the user is a moderator
        /// </summary>
        /// <param name="roomID"></param>
        /// <param name="messageID"></param>
        /// <returns></returns>
        [Authorize(Policy = "RequireModRole")]
        public async Task DeleteMessage(string roomID, string messageID)
        {
            Chatlog chatlog = db.Rooms.SingleOrDefault(u => u.ID == roomID).Chatlog;
            Message messageToDelete = chatlog.Messages.SingleOrDefault(u => u.ID == int.Parse(messageID));

            chatlog.Messages.Remove(messageToDelete);
            db.Update(chatlog);
            db.Remove(messageToDelete);
            db.SaveChanges(true);

            await Clients.Group(roomID).ShowMessages(chatlog.Messages);
        }

        /// <summary>
        /// When user disconnects, check if guest and then delete them from the db
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public override async Task OnDisconnectedAsync(Exception? exception)
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

        /// <summary>
        /// Update roomlist for everyone
        /// </summary>
        /// <returns></returns>
        private async Task SendRoomListUpdate()
        {
            await Clients.All.UpdateRoomList(db.Rooms.ToList());
        }

        /// <summary>
        /// Join a room
        /// </summary>
        /// <param name="RoomID"></param>
        /// <returns></returns>
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

            await Clients.Caller.RoomJoined(roomToJoin.Title, IsOwner);
            await Clients.Caller.ShowMessages(roomToJoin.Chatlog.Messages);
            await Clients.Caller.UpdateRoomList(db.Rooms.ToList());
        }

        /// <summary>
        /// Leave a room
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Add connectionid to group
        /// </summary>
        /// <param name="groupName"></param>
        /// <returns></returns>
        public async Task AddToGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }

        /// <summary>
        /// Remove connectionid from group
        /// </summary>
        /// <param name="groupName"></param>
        /// <returns></returns>
        public async Task RemoveFromGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        }
    }
}