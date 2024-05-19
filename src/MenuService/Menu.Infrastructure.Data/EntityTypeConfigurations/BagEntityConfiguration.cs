using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Menu.Domain;

namespace Menu.Infrastructure.Data.EntityTypeConfigurations
{
    public class BagEntityConfiguration : IEntityTypeConfiguration<Bag>
    {
        public void Configure(EntityTypeBuilder<Bag> builder)
        {

            builder.HasKey(cp => new { cp.UserId, cp.ProductId });



            builder.HasOne(x => x.User)
                .WithMany(x => x.Bags)
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.Product)
                .WithMany(x => x.Bags)
                .HasForeignKey(b => b.ProductId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
