using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CSSHarris.Models
{
    [NotMapped] //Will be generated when signalr connection established
    public class ChatUser
    {
        public IdentityUser? User { get; set; }

        public string? Username { get; set; }

        public List<ChatUser> Friends { get; set; }
        public List<ChatUser> FriendRequests { get; set; }

        public string ConnectionId { get; set; }

    }
}