﻿using Microsoft.AspNetCore.SignalR;
using CSSHarris.Models.ChatModels;
using ChatUser = CSSHarris.Models.ChatUser;

namespace CSSHarris.Hubs
{
    public class ChatHub : Hub<IConnectionHub>
    {
        private static readonly List<ChatUser> _Users = new();
        private static readonly List<Room> Rooms = new();

        public async Task SendMessage(string roomID, string message)
        {
            ChatUser signallingUser = _Users.Where(item => item.ConnectionId == Context.ConnectionId).FirstOrDefault();
            Room room = Rooms.SingleOrDefault(u => u.ID == roomID);

            room.Chatlog.Messages.Add(new Message() {
                ChatUser = signallingUser,
                DateTime= DateTime.Now,
                Content= message
            });

            await Clients.Group(roomID).ReceiveMessage(signallingUser.Username, message);
        }

        public async Task Join(string username)
        {
            // Add the new user
            ChatUser newUser = new()
            {
                Username = username,
                ConnectionId = Context.ConnectionId
            };

            _Users.Add(newUser);

            //Get rooms
            await Clients.Caller.UpdateRoomList(Rooms);
        }

        public async Task CreateRoom(string title)
        {
            // Create room
            Room room = new()
            {
                Owner = _Users.SingleOrDefault(u => u.ConnectionId == Context.ConnectionId),
                Title = title,
                ID = Context.ConnectionId + 1
            };

            Rooms.Add(room);

            // Send down the new list to all clients
            await SendRoomListUpdate();
        }

        public async Task DeleteRoom(string roomID)
        {
            var roomToDelete = Rooms.SingleOrDefault(u => u.ID == roomID);
            ChatUser signallingUser = _Users.Where(item => item.ConnectionId == Context.ConnectionId).FirstOrDefault();

            if (signallingUser != roomToDelete.Owner) return;


            await Clients.Group(roomID).UpdateUserList(null);
            await Clients.Group(roomID).RoomDeleted();

            foreach (ChatUser user in roomToDelete.UsersInRoom)
            {
                Groups.RemoveFromGroupAsync(user.ConnectionId, roomID);
            }

            Rooms.Remove(roomToDelete);

            // Send down the new list to all clients
            await SendRoomListUpdate();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var callingUser = _Users.SingleOrDefault(u => u.ConnectionId == Context.ConnectionId);

            if (callingUser == null)
            {
                return;
            }

            // Remove the user
            _Users.RemoveAll(u => u.ConnectionId == Context.ConnectionId);

            await base.OnDisconnectedAsync(exception);

            //Leave room
            var roomToLeave = Rooms.SingleOrDefault(u => u.UsersInRoom.Contains(callingUser));

            if (roomToLeave == null)
            {
                return;
            }
            roomToLeave.UsersInRoom.Remove(callingUser);

            await RemoveFromGroup(roomToLeave.ID);
            await Clients.Group(roomToLeave.ID).UpdateUserList(roomToLeave.UsersInRoom);
            await Clients.Caller.UpdateUserList(null);
            await Clients.Caller.UpdateRoomList(Rooms);
        }

        private async Task SendRoomListUpdate()
        {
            await Clients.All.UpdateRoomList(Rooms);
        }

        public async Task JoinRoom(string RoomID)
        {
            var callingUser = _Users.SingleOrDefault(u => u.ConnectionId == Context.ConnectionId);
            var roomToJoin = Rooms.SingleOrDefault(u => u.ID == RoomID);

            bool IsOwner = roomToJoin.Owner == callingUser ? true : false;

            //Join room
            roomToJoin.UsersInRoom.Add(callingUser);
            await AddToGroup(RoomID);
            await Clients.Group(RoomID).UpdateUserList(roomToJoin.UsersInRoom);

            //TODO do not send full user data
            await Clients.Caller.RoomJoined(roomToJoin.Title, IsOwner, roomToJoin.Chatlog.Messages);
            await Clients.Caller.UpdateRoomList(Rooms);
        }

        public async Task LeaveRoom()
        {
            var callingUser = _Users.SingleOrDefault(u => u.ConnectionId == Context.ConnectionId);

            if (callingUser == null) return;

            //Join room
            var roomToLeave = Rooms.SingleOrDefault(u => u.UsersInRoom.Contains(callingUser));

            roomToLeave.UsersInRoom.Remove(callingUser);

            await RemoveFromGroup(roomToLeave.ID);
            await Clients.Group(roomToLeave.ID).UpdateUserList(roomToLeave.UsersInRoom);
            await Clients.Caller.UpdateUserList(null);
            await Clients.Caller.UpdateRoomList(Rooms);
        }

        public async Task AddToGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }

        public async Task RemoveFromGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        }

        #region Friendrequests
        public async Task SendFriendRequest(string TargetConnectionId)
        {
            var callingUser = _Users.SingleOrDefault(u => u.ConnectionId == Context.ConnectionId);
            var target = _Users.Where(item => item.ConnectionId == TargetConnectionId).FirstOrDefault();
            if (target == null || callingUser == null) return;

            target.FriendRequests.Add(callingUser);
        }

        public async Task ConfirmFriendRequest(string TargetConnectionId)
        {
            var callingUser = _Users.SingleOrDefault(u => u.ConnectionId == Context.ConnectionId);
            var target = _Users.Where(item => item.ConnectionId == TargetConnectionId).FirstOrDefault();
            if (target == null || callingUser == null) return;

            target.Friends.Add(callingUser);
            callingUser.Friends.Add(target);
        }

        public async Task DenyFriendRequest(string TargetConnectionId)
        {
            var callingUser = _Users.SingleOrDefault(u => u.ConnectionId == Context.ConnectionId);
            var target = _Users.Where(item => item.ConnectionId == TargetConnectionId).FirstOrDefault();
            if (target == null || callingUser == null) return;

            callingUser.FriendRequests.Remove(target);
        }

        public async Task RevokeFriendRequest(string TargetConnectionId)
        {
            var callingUser = _Users.SingleOrDefault(u => u.ConnectionId == Context.ConnectionId);
            var target = _Users.Where(item => item.ConnectionId == TargetConnectionId).FirstOrDefault();
            if (target == null || callingUser == null) return;

            target.FriendRequests.Remove(callingUser);
        }
        #endregion
    }
}