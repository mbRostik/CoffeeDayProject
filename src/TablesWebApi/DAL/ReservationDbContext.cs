using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace TablesWebApi.DAL
{
    public class ReservationDbContext : DbContext
    {
        public DbSet<User> Users => Set<User>();
        public DbSet<Reservation> Reservations => Set<Reservation>();
        public DbSet<ReservationContact> ReservationContacts => Set<ReservationContact>();
        public DbSet<Table> Tables => Set<Table>();
        public DbSet<ReservationTableLink> ReservationTableLinks => Set<ReservationTableLink>();

        public ReservationDbContext(DbContextOptions<ReservationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>(e =>
            {
                e.HasKey(u => u.Id);
            });

            builder.Entity<Reservation>(e =>
            {
                e.HasKey(r => r.Id);
                e.HasOne(r => r.User)
                    .WithMany(u => u.Reservations)
                    .HasForeignKey(r => r.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                e.HasOne(r => r.Contact)
                    .WithOne(c => c.Reservation)
                    .HasForeignKey<ReservationContact>(c => c.ReservationId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<ReservationContact>(e =>
            {
                e.HasKey(c => c.Id);
                e.Property(c => c.Email).IsRequired();
                e.Property(c => c.Name).IsRequired();
                e.Property(c => c.PhoneNumber).IsRequired();
            });

            builder.Entity<Table>(e =>
            {
                e.HasKey(t => t.Id);
                e.HasIndex(t => t.TableNumber).IsUnique();
                e.Property(t => t.MaxSeats).IsRequired();

                e.HasData(
                    new Table { Id = Guid.Parse("00000000-0000-0000-0000-100000000001"), TableNumber = 1, MaxSeats = 2 },
                    new Table { Id = Guid.Parse("00000000-0000-0000-0000-000001000001"), TableNumber = 2, MaxSeats = 2 },
                    new Table { Id = Guid.Parse("00000000-0000-0000-0000-000003000002"), TableNumber = 3, MaxSeats = 4 },
                    new Table { Id = Guid.Parse("00000000-0000-0000-0000-000020000002"), TableNumber = 4, MaxSeats = 4 },
                    new Table { Id = Guid.Parse("00000000-0000-0000-0000-000000000003"), TableNumber = 5, MaxSeats = 6 },
                    new Table { Id = Guid.Parse("00000000-0000-0000-0000-000000700003"), TableNumber = 6, MaxSeats = 6 },
                    new Table { Id = Guid.Parse("00000000-0000-0000-0000-000003000004"), TableNumber = 7, MaxSeats = 4 },
                    new Table { Id = Guid.Parse("00000000-0000-0000-0000-000040000004"), TableNumber = 8, MaxSeats = 4 },
                    new Table { Id = Guid.Parse("00000000-0000-0000-0000-000000000009"), TableNumber = 9, MaxSeats = 8 },
                    new Table { Id = Guid.Parse("00000000-0000-0000-0000-000008000008"), TableNumber = 10, MaxSeats = 8 }
                );
            });

            builder.Entity<ReservationTableLink>(e =>
            {
                e.HasKey(rtl => rtl.Id);
                e.HasOne(rtl => rtl.Reservation)
                    .WithMany(r => r.ReservationTableLinks)
                    .HasForeignKey(rtl => rtl.ReservationId);

                e.HasOne(rtl => rtl.Table)
                    .WithMany(t => t.ReservationTableLinks)
                    .HasForeignKey(rtl => rtl.TableId);
            });
        }
    }
}
