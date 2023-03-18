using CSSHarris.Models.ChatModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CSSHarris.Models
{
    public class ChatUser
    {
        [Key]
        public string ChatUserID { get; set; }
        public string? UserID { get; set; }

        public string? ConnectionId { get; set; }
        public string UserName { get; set; }

        public List<ChatUser> Friends { get; set; }
        public List<ChatUser> IncomingRequests { get; set; }


        [ForeignKey("RoomId")]
        public Room? CurrentRoom { get; set; }
    }
}