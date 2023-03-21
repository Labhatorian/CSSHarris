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
        Task ReceiveMessage(int iD, string user, string message);
        Task UpdateRoomList(List<Room> rooms);
        Task RoomJoined(string roomtitle, bool isOwner);
        Task RoomDeleted();
        Task Connected(string? userName);
        Task ShowMessages(List<Message> messages);
    }
}
