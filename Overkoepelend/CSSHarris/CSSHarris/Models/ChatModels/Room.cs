using System.Security.Principal;

namespace CSSHarris.Models.ChatModels
{
    public class Room
    {
        public string ID { get; set; }
        public string Owner { get; set; }
        public string Title { get; set; }
        public List<ChatUser> UsersInRoom { get; set; } = new();
        public Chatlog Chatlog { get; set; } = new();
    }
}
