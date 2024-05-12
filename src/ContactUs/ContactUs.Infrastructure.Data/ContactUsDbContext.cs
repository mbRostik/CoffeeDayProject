using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContactUs.Domain;

namespace Users.Infrastructure.Data
{
    public class ContactUsDbContext : DbContext
    {
        public ContactUsDbContext(DbContextOptions options) : base(options) { }

        public DbSet<User> Users { get; set; }

        public DbSet<Message> Messages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ContactUsDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
