using Microsoft.AspNetCore.Identity;

namespace CSSHarris.Models
{
    public class User
    {
        public string Username { get; set; }
        public string ConnectionId { get; set; }
        public string Offer { get; set; }
        public bool InCall { get; set; }
    }
}
