using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CSSHarris.Models.ChatModels
{
    public class Chatlog
    {
        [Key] public int ID { get; set; }

        //todo check users max 2
        [ForeignKey("ID")]
        public List<ChatUser> Users { get; set; }

        public List<Message> Messages { get; set; } = new();
    }
}
