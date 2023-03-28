using CSSHarris.Hubs;
using CSSHarris.Models;
using CSSHarris.Models.ChatModels;

namespace MHeetTesting.Integration
{
    internal class MockConnectionHub : IConnectionHub
    {
        public Task ReceiveMessage(int iD, string user, string message)
        {
            return Task.CompletedTask;
        }

        public Task Connected(string? userName)
        {
            throw new NotImplementedException();
        }

        public Task RoomDeleted()
        {
            throw new NotImplementedException();
        }

        public Task RoomJoined(string roomtitle, bool isOwner)
        {
            throw new NotImplementedException();
        }

        public Task ShowMessages(List<Message> messages)
        {
            throw new NotImplementedException();
        }

        public Task UpdateRoomList(List<Room> rooms)
        {
            throw new NotImplementedException();
        }

        public Task UpdateUserList(List<ChatUser> userList)
        {
            throw new NotImplementedException();
        }
    }
}