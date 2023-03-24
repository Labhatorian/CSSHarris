using Microsoft.EntityFrameworkCore;
using NerdyGadgets.Models;

namespace NerdyGadgets.Data
{
    public class NerdyGadgetsDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ProductCategory> ProductsCategories { get; set; }

        public NerdyGadgetsDbContext(DbContextOptions<NerdyGadgetsDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Setup model (kan meer maar ook in model zelf met annotations)
            //modelBuilder.Entity<Product>().Property(p => p.ProductId).IsRequired();
            modelBuilder.Entity<Product>()
                .HasMany(c => c.ProductCategories)
                .WithOne(pc => pc.Product)
                .HasForeignKey(pc => pc.ProductId);

            modelBuilder.Entity<Category>()
                .HasMany(p => p.CategoryProducts)
                .WithOne(pc => pc.Category)
                .HasForeignKey(pc => pc.CategoryId);

            modelBuilder.Entity<ProductCategory>()
                .HasKey(pc => new { pc.ProductId, pc.CategoryId });

            ////Data seeding
            //var usb_category = new Category()
            //{
            //    CategoryNumber = 1,
            //    Name = "USB",
            //};
            //var tShirts_category = new Category()
            //{
            //    CategoryNumber = 2,
            //    Name = "T-Shirt",
            //};

            //var usbTshirt_product = new Product()
            //{
            //    ProductId = 1,
            //    Title = "USB Stick Tee Shirt",
            //    Price = 10,
            //    //Categories = new[] { usb_category,tShirts_category}
            //};

            ////maak twee ProductCategories aan en stel de foreign keys in!
            ////Voeg deze toe met HasData
            //var ProductCategoryUSB = new ProductCategory()
            //{
            //    CategoryId = 1,
            //    Category = usb_category,
            //    Product = usbTshirt_product,
            //    ProductId = 1,
            //};
            //var ProductCategoryTShirt = new ProductCategory()
            //{
            //    CategoryId = 2,
            //    Category = tShirts_category,
            //    Product = usbTshirt_product,
            //    ProductId = 1,
            //};

            //usbTshirt_product.Categories = new List<Category>() { usb_category,tShirts_category};
            //modelBuilder.Entity<Product>().HasData(usb_category, tShirts_category); niet nodig
            
            //modelBuilder.Entity<Product>().HasData(usbTshirt_product);
            //modelBuilder.Entity<Product>().HasData(ProductCategoryUSB);
            //modelBuilder.Entity<Product>().HasData(ProductCategoryTShirt);

            modelBuilder.Entity<Product>().HasData(
                new Product { ProductId = 1, Title = "USB Stick Tee Shirt", Price = 10, }
             );

            modelBuilder.Entity<Category>().HasData(
                new Category { CategoryNumber = 1, Name = "USB" },
                new Category { CategoryNumber = 2, Name = "T-Shirt" }
            );

            modelBuilder.Entity<ProductCategory>().HasData(
                new ProductCategory { ProductId = 1, CategoryId = 1 },
                new ProductCategory { ProductId = 1, CategoryId = 2 }
            );
        }
    }
}