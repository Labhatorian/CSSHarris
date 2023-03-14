namespace CSSHarris.Models.ChatModels
{
    public class Chatlog
    {
        //todo check users max 2
        public List<User> Users { get; set; }

        public List<Message> Messages { get; set; } = new();
    }
}
