using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace ShopWebApi.DAL
{
    public class ShopDbContext : DbContext
    {
        public ShopDbContext(DbContextOptions<ShopDbContext> options)
            : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<UserBag> UserBags { get; set; }
        public DbSet<UserBagProduct> UserBagProducts { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<UserOrder> UserOrders { get; set; }
        public DbSet<UserOrderProduct> UserOrderProducts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasKey(u => u.Id);

            modelBuilder.Entity<ProductCategory>()
                .HasKey(pc => new { pc.ProductId, pc.CategoryId });

            modelBuilder.Entity<ProductCategory>()
                .HasOne(pc => pc.Product)
                .WithMany(p => p.ProductCategories)
                .HasForeignKey(pc => pc.ProductId);

            modelBuilder.Entity<ProductCategory>()
                .HasOne(pc => pc.Category)
                .WithMany(c => c.ProductCategories)
                .HasForeignKey(pc => pc.CategoryId);

            modelBuilder.Entity<UserOrderProduct>()
                .HasKey(op => new { op.UserOrderId, op.ProductId });

            modelBuilder.Entity<UserOrderProduct>()
                .HasOne(op => op.UserOrder)
                .WithMany(o => o.OrderProducts)
                .HasForeignKey(op => op.UserOrderId);

            modelBuilder.Entity<UserOrderProduct>()
                .HasOne(op => op.Product)
                .WithMany(p => p.OrderProducts)
                .HasForeignKey(op => op.ProductId);

            modelBuilder.Entity<User>()
                .HasOne(u => u.Bag)
                .WithOne(b => b.User)
                .HasForeignKey<UserBag>(b => b.UserId);

            modelBuilder.Entity<UserBagProduct>()
                .HasKey(bp => new { bp.UserBagId, bp.ProductId });

            modelBuilder.Entity<UserBagProduct>()
                .HasOne(bp => bp.UserBag)
                .WithMany(b => b.BagProducts)
                .HasForeignKey(bp => bp.UserBagId);

            modelBuilder.Entity<UserBagProduct>()
                .HasOne(bp => bp.Product)
                .WithMany()
                .HasForeignKey(bp => bp.ProductId);


            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Coffee", Photo = GetImageBytes("CoffeeBag.png")},
                new Category { Id = 2, Name = "Coffee Maker", Photo = GetImageBytes("coffeemaker.png") },
                new Category { Id = 3, Name = "Cups", Photo = GetImageBytes("cup.png") },
                new Category { Id = 4, Name = "Accessories", Photo = GetImageBytes("acc.png") }
            );

            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Name = "Espresso Arabica", Description = "Strong, aromatic espresso beans.", Price = 12, Photo = GetImageBytes("ar.png") },
                new Product { Id = 2, Name = "French Press", Description = "Classic 1L French Press.", Price = 24, Photo = GetImageBytes("ar.png") },
                new Product { Id = 3, Name = "Latte Cup Set", Description = "Set of 4 ceramic latte cups.", Price = 18, Photo = GetImageBytes("ar.png") },
                new Product { Id = 4, Name = "Milk Frother", Description = "Electric milk frother for cappuccino.", Price = 29, Photo = GetImageBytes("ar.png") },
                new Product { Id = 5, Name = "Colombian Roast", Description = "Smooth medium roast coffee.", Price = 10, Photo = GetImageBytes("ar.png") }
            );

            modelBuilder.Entity<ProductCategory>().HasData(
                new ProductCategory { ProductId = 1, CategoryId = 1 },
                new ProductCategory { ProductId = 2, CategoryId = 2 },
                new ProductCategory { ProductId = 3, CategoryId = 3 },
                new ProductCategory { ProductId = 4, CategoryId = 4 },
                new ProductCategory { ProductId = 5, CategoryId = 1 }
            );
        }

        public static byte[] GetImageBytes(string relativePath)
        {
            var absolutePath = Path.Combine("D:\\C\\Projects\\CoffeeDay\\ShopWebApi", "Images", relativePath);
            return File.ReadAllBytes(absolutePath);
        }
    }
}
