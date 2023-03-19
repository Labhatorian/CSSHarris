using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace CSSHarris.Models
{
    public class ApplicationUser : IdentityUser
    {
        public bool Banned { get; set; }

    }
}
