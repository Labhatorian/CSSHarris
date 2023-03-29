using System.ComponentModel.DataAnnotations;

namespace CSSHarris.Models.ChatModels
{
    public class Message
    {
        [Key]
        public int ID { get; set; }

        public string Username { get; set; }

        [MaxLength(64)]
        [StringLength(64)]
        public string Content { get; set; }

    }
}