using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Principal;

namespace CSSHarris.Models
{
    public class ChatUser
    {
        [ForeignKey("ID")]
        public string? UserID { get; set; }

        public string UserName { get; set; }

        public List<ChatUser> Friends { get; set; } = new();

        public List<ChatUser> FriendRequests { get; set; } = new();

        public string ConnectionId { get; set; }

    }
}