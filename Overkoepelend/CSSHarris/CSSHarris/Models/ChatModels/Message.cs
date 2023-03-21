using System.ComponentModel.DataAnnotations;

namespace CSSHarris.Models.ChatModels
{
    public class Message
    {
        [Key]
        public int ID { get; set; }

        public string Username { get; set; }
        public string Content { get; set; }

    }
}