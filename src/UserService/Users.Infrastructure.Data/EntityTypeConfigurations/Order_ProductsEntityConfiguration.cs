using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users.Domain;

namespace Users.Infrastructure.Data.EntityTypeConfigurations
{
    public class Order_ProductsEntityConfiguration : IEntityTypeConfiguration<Order_Products>
    {
        public void Configure(EntityTypeBuilder<Order_Products> builder)
        {
            builder.HasKey(cp => new { cp.ProductId, cp.OrderId });


            builder.HasOne(x => x.Product)
                .WithMany(x => x.Order_Products)
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.Order)
                .WithMany(x => x.Order_Products)
                .HasForeignKey(x => x.OrderId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
