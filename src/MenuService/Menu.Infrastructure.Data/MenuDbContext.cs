using Menu.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Menu.Infrastructure.Data
{
    public class MenuDbContext : DbContext
    {
        public MenuDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Bag> Bags { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<CategoryWithProduct> CategoriesWithProducts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(MenuDbContext).Assembly);
            modelBuilder.Entity<Category>().HasData(
                        new Category { Id = 1, Name = "Water", Photo = GetImageBytes("water-bottle.png") },
                        new Category { Id = 2, Name = "Coffee", Photo = GetImageBytes("coffee-cup.png") },
                        new Category { Id = 3, Name = "Smoothie", Photo = GetImageBytes("banana.png") },
                        new Category { Id = 4, Name = "MilkShake", Photo = GetImageBytes("milkshake.png") },
                        new Category { Id = 5, Name = "Pressed juice", Photo = GetImageBytes("wine-press.png") },
                        new Category { Id = 6, Name = "Salad", Photo = GetImageBytes("salad.png") },
                        new Category { Id = 7, Name = "Soup", Photo = GetImageBytes("winter-melon-soup.png") },
                        new Category { Id = 9, Name = "Sandwich", Photo = GetImageBytes("bread.png") },
                        new Category { Id = 10, Name = "Soft Drinks", Photo = GetImageBytes("soft-drink.png") }
                    );

            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Name = "Water", Price = 1, Description = "Just a water", Photo = new byte[0] },
                new Product { Id = 2, Name = "Americano", Price = 2, Description = "Classic Americano coffee", Photo = new byte[0] },
                new Product { Id = 3, Name = "Latte", Price = 3, Description = "Rich and creamy latte", Photo = new byte[0] },
                new Product { Id = 4, Name = "Espresso", Price = 2, Description = "Strong and bold espresso", Photo = new byte[0] },
                new Product { Id = 5, Name = "Smoothie Mix", Price = 5, Description = "Healthy fruit smoothie", Photo = new byte[0] },
                new Product { Id = 6, Name = "Vanilla Milkshake", Price = 4, Description = "Delicious vanilla milkshake", Photo = new byte[0] },
                new Product { Id = 7, Name = "Lemon", Price = 3, Description = "Fresh lemon", Photo = new byte[0] },
                new Product { Id = 8, Name = "Caesar Salad", Price = 6, Description = "Classic Caesar salad", Photo = new byte[0] },
                new Product { Id = 9, Name = "Tomato Soup", Price = 4, Description = "Warm tomato soup", Photo = new byte[0] },
                new Product { Id = 10, Name = "Pancake Breakfast", Price = 7, Description = "Pancakes with syrup", Photo = new byte[0] },
                new Product { Id = 11, Name = "Club Sandwich", Price = 5, Description = "Classic club sandwich", Photo = new byte[0] },
                new Product { Id = 12, Name = "Cola", Price = 2, Description = "Chilled cola", Photo = new byte[0] }
            );

            modelBuilder.Entity<CategoryWithProduct>().HasData(
                new CategoryWithProduct { CategoryId = 1, ProductId = 1 },
                new CategoryWithProduct { CategoryId = 2, ProductId = 2 },
                new CategoryWithProduct { CategoryId = 2, ProductId = 3 },
                new CategoryWithProduct { CategoryId = 2, ProductId = 4 },
                new CategoryWithProduct { CategoryId = 3, ProductId = 5 },
                new CategoryWithProduct { CategoryId = 4, ProductId = 6 },
                new CategoryWithProduct { CategoryId = 5, ProductId = 7 },
                new CategoryWithProduct { CategoryId = 6, ProductId = 8 },
                new CategoryWithProduct { CategoryId = 7, ProductId = 9 },
                new CategoryWithProduct { CategoryId = 9, ProductId = 11 },
                new CategoryWithProduct { CategoryId = 10, ProductId = 12 }
            );
            base.OnModelCreating(modelBuilder);
        }

        public static byte[] GetImageBytes(string relativePath)
        {
            var absolutePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "images", relativePath);
            return File.ReadAllBytes(absolutePath);
        }

    }
}

