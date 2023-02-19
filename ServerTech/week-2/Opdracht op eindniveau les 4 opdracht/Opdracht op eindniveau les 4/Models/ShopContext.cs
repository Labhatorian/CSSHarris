using Microsoft.EntityFrameworkCore;

namespace Opdracht_op_eindniveau_les_4.Models
{
    public class ShopContext : DbContext
    {
        public DbSet<Product> Products { get; set;}
        public DbSet<Category> Categories { get; set;}

        public ShopContext(DbContextOptions<ShopContext> options) : base(options) { }
    }
}