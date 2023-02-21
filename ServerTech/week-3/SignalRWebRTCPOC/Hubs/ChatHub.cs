using Microsoft.AspNetCore.SignalR;
using SignalRWebRTCPOC.Models;

namespace SignalRWebRTCPOC.Hubs
{
    public class ChatHub : Hub<IConnectionHub>
    {
        private static readonly List<User> _Users = new();
        private static readonly List<UserCall> _UsersInCall = new();
        private static readonly List<CallOffer> _CallOffers = new();

        public async Task SendMessage(string signallinguserID, string message)
        {
            User signallingUser = _Users.Where(item => item.ConnectionId == signallinguserID).FirstOrDefault();
            UserCall targetCall = GetUserCall(signallinguserID);
            User targetUser = targetCall.Users.Where(user => user.ConnectionId != signallinguserID).FirstOrDefault();

            await Clients.Client(signallinguserID).ReceiveMessage(signallingUser.Username, message);
            await Clients.Client(targetUser.ConnectionId).ReceiveMessage(signallingUser.Username, message);
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

            // Send down the new list to all clients
            await SendUserListUpdate();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            // Remove the user
            _Users.RemoveAll(u => u.ConnectionId == Context.ConnectionId);

            // Send down the new user list to all clients
            await SendUserListUpdate();

            await base.OnDisconnectedAsync(exception);
        }

        private async Task SendUserListUpdate()
        {
            //_Users.ForEach(u => u.InCall = (GetUserCall(u.ConnectionId) != null));

            await Clients.All.UpdateUserList(_Users);
        }
        private UserCall GetUserCall(string connectionId)
        {
            var matchingCall =
                _UsersInCall.SingleOrDefault(uc => uc.Users.SingleOrDefault(u => u.ConnectionId == connectionId) != null);
            return matchingCall;
        }

        public async Task CallUser(User targetConnectionId)
        {
            var callingUser = _Users.SingleOrDefault(u => u.ConnectionId == Context.ConnectionId);
            var targetUser = _Users.SingleOrDefault(u => u.ConnectionId == targetConnectionId.ConnectionId);

            // Make sure the person we are trying to call is still here
            if (targetUser == null)
            {
                // If not, let the caller know
                await Clients.Caller.CallDeclined(targetConnectionId, "The user you called has left.");
                return;
            }

            // And that they aren't already in a call
            if (GetUserCall(targetUser.ConnectionId) != null)
            {
                await Clients.Caller.CallDeclined(targetConnectionId, string.Format("{0} is already in a call.", targetUser.Username));
                return;
            }

            // They are here, so tell them someone wants to talk
            await Clients.Client(targetConnectionId.ConnectionId).IncomingCall(callingUser);

            // Create an offer
            _CallOffers.Add(new CallOffer
            {
                Caller = callingUser,
                Callee = targetUser
            });
        }

        public async Task AnswerCall(bool acceptCall, User targetConnectionId)
        {
            var callingUser = _Users.SingleOrDefault(u => u.ConnectionId == Context.ConnectionId);
            var targetUser = _Users.SingleOrDefault(u => u.ConnectionId == targetConnectionId.ConnectionId);

            // This can only happen if the server-side came down and clients were cleared, while the user
            // still held their browser session.
            if (callingUser == null)
            {
                return;
            }

            // Make sure the original caller has not left the page yet
            if (targetUser == null)
            {
                await Clients.Caller.CallEnded(targetConnectionId, "The other user in your call has left.");
                return;
            }

            // Send a decline message if the callee said no
            if (acceptCall == false)
            {
                await Clients.Client(targetConnectionId.ConnectionId).CallDeclined(callingUser, string.Format("{0} did not accept your call.", callingUser.Username));
                return;
            }

            // Make sure there is still an active offer.  If there isn't, then the other use hung up before the Callee answered.
            var offerCount = _CallOffers.RemoveAll(c => c.Callee.ConnectionId == callingUser.ConnectionId
                                                  && c.Caller.ConnectionId == targetUser.ConnectionId);
            if (offerCount < 1)
            {
                await Clients.Caller.CallEnded(targetConnectionId, string.Format("{0} has already hung up.", targetUser.Username));
                return;
            }

            // And finally... make sure the user hasn't accepted another call already
            if (GetUserCall(targetUser.ConnectionId) != null)
            {
                // And that they aren't already in a call
                await Clients.Caller.CallDeclined(targetConnectionId, string.Format("{0} chose to accept someone elses call instead of yours :(", targetUser.Username));
                return;
            }

            // Remove all the other offers for the call initiator, in case they have multiple calls out
            _CallOffers.RemoveAll(c => c.Caller.ConnectionId == targetUser.ConnectionId);

            // Create a new call to match these folks up
            _UsersInCall.Add(new UserCall
            {
                Users = new List<User> { callingUser, targetUser }
            });

            // Tell the original caller that the call was accepted
            await Clients.Client(targetConnectionId.ConnectionId).CallAccepted(callingUser);

            // Update the user list, since thes two are now in a call
            await SendUserListUpdate();
        }

        public async Task HangUp()
        {
            var callingUser = _Users.SingleOrDefault(u => u.ConnectionId == Context.ConnectionId);

            if (callingUser == null)
            {
                return;
            }

            var currentCall = GetUserCall(callingUser.ConnectionId);

            // Send a hang up message to each user in the call, if there is one
            if (currentCall != null)
            {
                foreach (var user in currentCall.Users.Where(u => u.ConnectionId != callingUser.ConnectionId))
                {
                    await Clients.Client(user.ConnectionId).CallEnded(callingUser, string.Format("{0} has hung up.", callingUser.Username));
                }

                // Remove the call from the list if there is only one (or none) person left.  This should
                // always trigger now, but will be useful when we implement conferencing.
                currentCall.Users.RemoveAll(u => u.ConnectionId == callingUser.ConnectionId);
                if (currentCall.Users.Count < 2)
                {
                    _UsersInCall.Remove(currentCall);
                }
            }

            // Remove all offers initiating from the caller
            _CallOffers.RemoveAll(c => c.Caller.ConnectionId == callingUser.ConnectionId);

            await SendUserListUpdate();
        }

    }

    public interface IConnectionHub
    {
        Task UpdateUserList(List<User> userList);
        Task CallAccepted(User acceptingUser);
        Task CallDeclined(User decliningUser, string reason);
        Task IncomingCall(User callingUser);
        Task ReceiveSignal(User signalingUser, string signal);
        Task CallEnded(User signalingUser, string signal);
        Task SendMessage(User signalingUser, string signal);
        Task SendAsync(User signalingUser, string signal);
        Task ReceiveMessage(string user, string message);
    }
}