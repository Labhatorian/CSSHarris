namespace Setup.Models.ChatModels
{
    public class Room
    {
        public string ID { get; set; }
        public User Owner { get; set; }
        public string Title { get; set; }
        public List<User> UsersInRoom { get; set; } = new();
        public Chatlog Chatlog { get; set; } = new();
    }
}
