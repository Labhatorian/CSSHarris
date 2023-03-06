using Setup.Models;
using Setup.Models.ChatModels;

namespace Setup.Hubs
{
    public interface IConnectionHub
    {
        Task UpdateUserList(List<User> userList);
        Task CallAccepted(User acceptingUser);
        Task CallDeclined(User decliningUser, string reason);
        Task IncomingCall(User callingUser);
        Task ReceiveSignal(User signalingUser, string signal);
        Task CallEnded(User signalingUser, string signal);
        Task SendMessage(string roomid, string message);
        Task SendAsync(User signalingUser, string signal);
        Task ReceiveMessage(string user, string message);
        Task UpdateRoomList(List<Room> rooms);
        Task RoomJoined(string roomtitle);
    }
}
