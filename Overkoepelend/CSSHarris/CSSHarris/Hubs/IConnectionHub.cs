using CSSHarris.Models;
using CSSHarris.Models.ChatModels;

namespace CSSHarris.Hubs
{
    public interface IConnectionHub
    {
        Task UpdateUserList(List<ChatUser> userList);
        Task ReceiveMessage(int iD, string user, string message);
        Task UpdateRoomList(List<Room> rooms);
        Task RoomJoined(string roomtitle, bool isOwner);
        Task RoomDeleted();
        Task Connected(string? userName);
        Task ShowMessages(List<Message> messages);
    }
}