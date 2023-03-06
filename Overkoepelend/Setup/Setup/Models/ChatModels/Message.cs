namespace Setup.Models.ChatModels
{
    public class Message
    {
        public User User { get; set; }
        public DateTime DateTime { get; set; }
        public string Content { get; set; }
    }
}