using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace CSSHarris.Models.ChatModels
{
    public class Message
    {
        [Key] public int ID { get; set; }
        public ChatUser ChatUser { get; set; }
        public DateTime DateTime { get; set; }
        public string Content { get; set; }

    }
}