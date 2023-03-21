using System.ComponentModel.DataAnnotations;

namespace CSSHarris.Models.ChatModels
{
    public class Chatlog
    {
        [Key] public int ID { get; set; }

        public List<Message> Messages { get; set; } = new();
    }
}
