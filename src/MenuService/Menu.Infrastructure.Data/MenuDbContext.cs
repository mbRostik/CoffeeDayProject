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
               new Category { Id = 1, Name = "Water", Photo = [] },
               new Category { Id = 2, Name = "Coffee", Photo = [] },
               new Category { Id = 3, Name = "Smoothie", Photo = [] },
               new Category { Id = 4, Name = "MilkShake", Photo = [] },
               new Category { Id = 5, Name = "Iced Coffee", Photo = [] },
               new Category { Id = 6, Name = "Salad", Photo = [] },
               new Category { Id = 7, Name = "Soup", Photo = [] },
               new Category { Id = 8, Name = "Breakfast", Photo = [] },
               new Category { Id = 9, Name = "Sandwich", Photo = [] },
               new Category { Id = 10, Name = "Soft Drinks", Photo = [] }
           );

            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Name = "Water", Price = 1, Description="Just a water", Photo = [] },
                new Product { Id = 2, Name = "Amerecano", Price = 2, Description="Ia ne shariy za cofe", Photo = [] }
            );

            modelBuilder.Entity<CategoryWithProduct>().HasData(
                new CategoryWithProduct { CategoryId = 1, ProductId = 1 },
                new CategoryWithProduct { CategoryId = 2, ProductId = 2 }
            );

            base.OnModelCreating(modelBuilder);
        }
    }
}

