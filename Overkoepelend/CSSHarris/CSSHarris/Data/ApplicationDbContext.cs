using CSSHarris.Models.ChatModels;
using CSSHarris.Models.DeveloperModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CSSHarris.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<Email> Emails { get; set; }
        public DbSet<Chatlog> Chatlogs { get; set; }
        public DbSet<Room> Rooms { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // put your fluent API code here
            base.OnModelCreating(modelBuilder); // call base method
        }
    }
}