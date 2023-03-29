using CijferRegistratie.Models;
using Microsoft.EntityFrameworkCore;

namespace CijferRegistratie.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Vak> Vakken { get; set; }
        public DbSet<Poging> Pogingen { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Vak>().HasData(new Vak()
            {
                VakId = 1,
                Naam = "Server",
                EC = 4
            }); ;

            modelBuilder.Entity<Vak>().HasData(new Vak()
            {
                VakId = 2,
                Naam = "C#",
                EC = 4
            });

            modelBuilder.Entity<Vak>().HasData(new Vak()
            {
                VakId = 3,
                Naam = "Databases",
                EC = 3
            });

            modelBuilder.Entity<Vak>().HasData(new Vak()
            {
                VakId = 4,
                Naam = "UML",
                EC = 3
            });

            modelBuilder.Entity<Vak>().HasData(new Vak()
            {
                VakId = 5,
                Naam = "KBS",
                EC = 9
            });

            //One-to-Many
            modelBuilder.Entity<Poging>()
                .HasOne<Vak>(s => s.Vak)
                .WithMany(g => g.Pogingen)
                .HasForeignKey(s => s.VakId);
        }
    }
}