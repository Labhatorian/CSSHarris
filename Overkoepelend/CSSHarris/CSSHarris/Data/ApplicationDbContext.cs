using CSSHarris.Models;
using CSSHarris.Models.ChatModels;
using CSSHarris.Models.DeveloperModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CSSHarris.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<Email> Emails { get; set; }

        public DbSet<ChatUser> ChatUsers { get; set; }
        public DbSet<Chatlog> Chatlogs { get; set; }
        public DbSet<Room> Rooms { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Room>().Navigation(r => r.Chatlog).AutoInclude();
            modelBuilder.Entity<Chatlog>().Navigation(r => r.Messages).AutoInclude();

            modelBuilder.Entity<Chatlog>()
                .Property(e => e.ID)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Message>()
                .Property(e => e.ID)
            .ValueGeneratedOnAdd();

            base.OnModelCreating(modelBuilder); 
        }
    }
}