using CSSHarris.Models.ChatModels;
using CSSHarris.Models.DeveloperModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CSSHarris.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        //TODO Customdatabase???
        public DbSet<Email> Emails { get; set; }
        public DbSet<Chatlog> Chatlogs { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}