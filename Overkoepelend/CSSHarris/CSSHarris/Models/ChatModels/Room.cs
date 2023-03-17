using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Principal;

namespace CSSHarris.Models.ChatModels
{
    public class Room
    {
        [Key] 
        public string ID { get; set; }

        public string Owner { get; set; }
        public string Title { get; set; }
        public Chatlog Chatlog { get; set; } = new();
    }
}