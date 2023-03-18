using Microsoft.AspNetCore.SignalR;
using CSSHarris.Models.ChatModels;
using ChatUser = CSSHarris.Models.ChatUser;
using Microsoft.AspNetCore.Authorization;
using System.Security.Principal;
using System.Security.Claims;
using CSSHarris.Data;
using CSSHarris.Models;
using System.Collections.Concurrent;
using Microsoft.EntityFrameworkCore;

namespace CSSHarris.Hubs
{
    [Authorize]
    public class FriendHub : Hub<IConnectionHub>
    {
        private readonly ApplicationDbContext db;

        public FriendHub(ApplicationDbContext db)
        {
            this.db = db;
        }

        public override async Task OnConnectedAsync()
        {
            List<ChatUserVM>? requests, friends;
            GetAllFriends(out requests, out friends);
            await Clients.Caller.GetAllFriends(requests, friends);

            await base.OnConnectedAsync();
        }

        private void GetAllFriends(out List<ChatUserVM>? requests, out List<ChatUserVM>? friends)
        {
            IIdentity currentUser = Context.User.Identity;

            var identity = (ClaimsIdentity)Context.User.Identity;
            var userIdClaim = identity.FindFirst(ClaimTypes.NameIdentifier);
            var userId = (userIdClaim is not null) ? userIdClaim.Value : null;

            var user = db.ChatUsers.Include(i => i.IncomingRequests).Where(user => user.UserID == userId).AsNoTracking().FirstOrDefault();
            requests = user?.IncomingRequests?.Select(f => new ChatUserVM
            {
                ChatUserID = f.ChatUserID,
                UserName = f.UserName
            }).ToList();
            user = db.ChatUsers.Include(i => i.Friends).Where(user => user.UserID == userId).AsNoTracking().FirstOrDefault();
            friends = user?.Friends?.Select(f => new ChatUserVM
            {
                ChatUserID = f.ChatUserID,
                UserName = f.UserName
            }).ToList();
        }

        public async Task DecideFriend(string TargetUserID, bool AddFriend)
        {
            IIdentity currentUser = Context.User.Identity;

            var identity = (ClaimsIdentity)Context.User.Identity;
            var userIdClaim = identity.FindFirst(ClaimTypes.NameIdentifier);
            var userId = (userIdClaim is not null) ? userIdClaim.Value : null;

            var maincallingUser = db.ChatUsers.SingleOrDefault(u => u.UserID == userId);
            var maintarget = db.ChatUsers.Where(item => item.ChatUserID == TargetUserID).FirstOrDefault();

            var callingUser = db.ChatUsers.Include(c => c.IncomingRequests).Where(user => user.UserID == userId).FirstOrDefault();
            callingUser.IncomingRequests.Remove(maintarget);
            db.Update(callingUser);
            db.SaveChanges();

            if (AddFriend)
            {
                var callingUserAdd = db.ChatUsers.Include(c => c.Friends).Where(user => user.UserID == userId).FirstOrDefault();
                callingUserAdd.Friends.Add(maintarget);
                db.Update(callingUserAdd);
                db.SaveChanges();

                var target = db.ChatUsers.Include(c => c.Friends).Where(user => user.ChatUserID == TargetUserID).FirstOrDefault();
                target.Friends.Add(callingUser);
                db.Update(target);
                db.SaveChanges();
            }

            List<ChatUserVM>? requests, friends;
            GetAllFriends(out requests, out friends);
            await Clients.Caller.GetAllFriends(requests, friends);
        }

        public async Task DeleteFriend(string TargetUserID)
        {
            IIdentity currentUser = Context.User.Identity;

            var identity = (ClaimsIdentity)Context.User.Identity;
            var userIdClaim = identity.FindFirst(ClaimTypes.NameIdentifier);
            var userId = (userIdClaim is not null) ? userIdClaim.Value : null;

            var callingUser = db.ChatUsers.SingleOrDefault(u => u.UserID == userId);
            var target = db.ChatUsers.Where(item => item.ChatUserID == TargetUserID).FirstOrDefault();

            callingUser = db.ChatUsers.Include(c => c.Friends).Where(user => user.UserID == userId).FirstOrDefault();
            callingUser.Friends.Remove(target);

            target = db.ChatUsers.Include(c => c.Friends).Where(user => user.ChatUserID == TargetUserID).FirstOrDefault();
            target.Friends.Remove(callingUser);

            db.Update(target);
            db.Update(callingUser);
            db.SaveChanges();

            List<ChatUserVM>? requests, friends;
            GetAllFriends(out requests, out friends);
            await Clients.Caller.GetAllFriends(requests, friends);
        }
    }
}