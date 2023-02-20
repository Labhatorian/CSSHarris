using Microsoft.EntityFrameworkCore;
using Scaffolding.Models;

namespace Scaffolding.DAL
{
    public class ScaffoldingContext : DbContext
    {
        public ScaffoldingContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Student> Students { get; set; }
    }
}
