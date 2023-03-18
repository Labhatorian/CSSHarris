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
            IIdentity currentUser = Context.User.Identity;

            var identity = (ClaimsIdentity)Context.User.Identity;
            var userIdClaim = identity.FindFirst(ClaimTypes.NameIdentifier);
            var userId = (userIdClaim is not null) ? userIdClaim.Value : null;

            var user = db.ChatUsers.Include(i => i.IncomingRequests).Where(user => user.UserID == userId).AsNoTracking().FirstOrDefault();
            var requests = user?.IncomingRequests?.Select(f => new ChatUserVM
            {
                UserName = f.UserName
            }).ToList();

            user = db.ChatUsers.Include(i => i.Friends).Where(user => user.UserID == userId).AsNoTracking().FirstOrDefault();
            var friends = user?.Friends?.Select(f => new ChatUserVM
            {
                UserName = f.UserName
            }).ToList();

            await Clients.Caller.GetAllFriends(requests, friends);

            await base.OnConnectedAsync();
        }

    }
}