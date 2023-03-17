using CSSHarris.Models.ChatModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata;

namespace CSSHarris.Models
{
    public class ChatUser
    {
        [Key]
        public string ID { get; set; }
        public string? UserID { get; set; }

        public string? ConnectionId { get; set; }
        public string UserName { get; set; }

        public List<ChatUser> Friends { get; set; } = new();
        public List<ChatUser> FriendRequests { get; set; } = new();

        [ForeignKey("RoomId")]
        public Room? CurrentRoom { get; set; }
    }
}