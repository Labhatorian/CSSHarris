using Microsoft.EntityFrameworkCore;
using TakenlijstManager.Data.Entities;

namespace TakenlijstManager.Data
{
    public class TakenManagerDbContext: DbContext
    {
        public DbSet<Taak> Taken { get; set; }
        public DbSet<Status> Statussen { get; set; }

        public TakenManagerDbContext(DbContextOptions<TakenManagerDbContext> options): base(options)
        {

        }
    }
}
