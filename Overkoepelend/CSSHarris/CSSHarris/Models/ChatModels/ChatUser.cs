using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Principal;

namespace CSSHarris.Models
{
    [NotMapped] //Will be generated when signalr connection established
    public class ChatUser
    {
        public IIdentity? User { get; set; }

        public string? Username { get; set; }

        public List<ChatUser> Friends { get; set; }
        public List<ChatUser> FriendRequests { get; set; }

        public string ConnectionId { get; set; }

    }
}