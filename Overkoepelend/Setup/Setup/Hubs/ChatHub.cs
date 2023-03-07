using Mailjet.Client.Resources;
using Microsoft.AspNetCore.SignalR;
using Setup.Models;
using Setup.Models.ChatModels;
using User = Setup.Models.User;

namespace Setup.Hubs
{
    public class ChatHub : Hub<IConnectionHub>
    {
        private static readonly List<User> _Users = new();
        private static readonly List<Room> Rooms = new();

        public async Task SendMessage(string roomID, string message)
        {
            User signallingUser = _Users.Where(item => item.ConnectionId == Context.ConnectionId).FirstOrDefault();
            Room room = Rooms.SingleOrDefault(u => u.ID == roomID);

            await Clients.Group(roomID).ReceiveMessage(signallingUser.Username, message);
        }

        public async Task Join(string username)
        {
            // Add the new user
            User newUser = new()
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
            User signallingUser = _Users.Where(item => item.ConnectionId == Context.ConnectionId).FirstOrDefault();

            if (signallingUser != roomToDelete.Owner) return;


            await Clients.Group(roomID).UpdateUserList(null);
            await Clients.Group(roomID).RoomDeleted();

            foreach (User user in roomToDelete.UsersInRoom)
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

            await Clients.Caller.RoomJoined(roomToJoin.Title, IsOwner);
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
    }
}