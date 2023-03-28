using Microsoft.AspNetCore.Identity;

namespace CSSHarris.Models
{
    public class ApplicationUser : IdentityUser
    {
        public bool Banned { get; set; }
    }
}