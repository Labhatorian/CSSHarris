using Microsoft.AspNetCore.Identity;

namespace Setup.Models
{
    public class User
    {
        public string Username { get; set; }
        public string ConnectionId { get; set; }
        public string Offer { get; set; }
        public bool InCall { get; set; }
    }
}
