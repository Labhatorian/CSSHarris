using CSSHarris.Models;
using CSSHarris.Models.ChatModels;
using CSSHarris.Models.DeveloperModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using System.Net;
using System;
using System.Reflection.Metadata;

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

            modelBuilder.Entity<ChatUser>().Navigation(r => r.CurrentRoom).AutoInclude();

            modelBuilder.Entity<Chatlog>()
                .Property(e => e.ID)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Message>()
                .Property(e => e.ID)
            .ValueGeneratedOnAdd();

            base.OnModelCreating(modelBuilder);

            modelBuilder.HasDefaultSchema("Identity");
            modelBuilder.Entity<IdentityUser>(entity =>
            {
                entity.ToTable(name: "User");
            });
            modelBuilder.Entity<IdentityRole>(entity =>
            {
                entity.ToTable(name: "Role");
            });
            modelBuilder.Entity<IdentityUserRole<string>>(entity =>
            {
                entity.ToTable("UserRoles");
            });
            modelBuilder.Entity<IdentityUserClaim<string>>(entity =>
            {
                entity.ToTable("UserClaims");
            });
            modelBuilder.Entity<IdentityUserLogin<string>>(entity =>
            {
                entity.ToTable("UserLogins");
            });
            modelBuilder.Entity<IdentityRoleClaim<string>>(entity =>
            {
                entity.ToTable("RoleClaims");
            });
            modelBuilder.Entity<IdentityUserToken<string>>(entity =>
            {
                entity.ToTable("UserTokens");
            });
        }
    }
}