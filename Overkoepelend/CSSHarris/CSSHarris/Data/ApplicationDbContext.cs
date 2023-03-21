using CSSHarris.Models;
using CSSHarris.Models.ChatModels;
using CSSHarris.Models.DeveloperModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CSSHarris.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
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

            //Change tablenames to something better
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

            //Roles
            modelBuilder.Entity<IdentityRole>().HasData(new IdentityRole { Id = "a18be9c0-aa65-4af8-bd17-00bd9320e575", Name = "Admin", NormalizedName = "Admin".ToUpper() });
            modelBuilder.Entity<IdentityRole>().HasData(new IdentityRole { Id = "a18be9c0-aa65-4af8-bd17-00ca3234e243", Name = "Moderator", NormalizedName = "Moderator".ToUpper() });

            //Dataseed for testing
            const string ADMIN_ID = "a18be9c0-aa65-4af8-bd17-00bd9344e575";

            var hasher = new PasswordHasher<ApplicationUser>();
            modelBuilder.Entity<ApplicationUser>().HasData(new ApplicationUser
            {
                Id = ADMIN_ID,
                UserName = "admin",
                NormalizedUserName = "admin",
                Email = "admin@admin.nl",
                NormalizedEmail = "admin@admin.nl,",
                EmailConfirmed = true,
                PasswordHash = hasher.HashPassword(null, "Admin123!"),
                SecurityStamp = string.Empty
            });

            modelBuilder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
            {
                RoleId = "a18be9c0-aa65-4af8-bd17-00bd9320e575",
                UserId = ADMIN_ID
            });
        }
    }
}