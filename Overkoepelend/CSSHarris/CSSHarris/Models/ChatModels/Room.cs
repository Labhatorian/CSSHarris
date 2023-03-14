namespace CSSHarris.Models.ChatModels
{
    public class Room
    {
        public string ID { get; set; }
        public ChatUser Owner { get; set; }
        public string Title { get; set; }
        public List<ChatUser> UsersInRoom { get; set; } = new();
        public Chatlog Chatlog { get; set; } = new();
    }
}
