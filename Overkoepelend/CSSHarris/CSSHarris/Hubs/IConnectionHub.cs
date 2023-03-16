using CSSHarris.Models;
using CSSHarris.Models.ChatModels;

namespace CSSHarris.Hubs
{
    public interface IConnectionHub
    {
        Task UpdateUserList(List<ChatUser> userList);
        Task CallAccepted(ChatUser acceptingUser);
        Task CallDeclined(ChatUser decliningUser, string reason);
        Task IncomingCall(ChatUser callingUser);
        Task ReceiveSignal(ChatUser signalingUser, string signal);
        Task CallEnded(ChatUser signalingUser, string signal);
        Task SendMessage(string roomid, string message);
        Task SendAsync(ChatUser signalingUser, string signal);
        Task ReceiveMessage(string user, string message);
        Task UpdateRoomList(List<Room> rooms);
        Task RoomJoined(string roomtitle, bool isOwner, List<Message> messages);
        Task RoomDeleted();
        Task Connected(string? userName);
    }
}
