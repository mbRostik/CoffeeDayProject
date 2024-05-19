using Menu.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Menu.Infrastructure.Data.EntityTypeConfigurations
{
    public class CategoryWithProductEntityConfiguration : IEntityTypeConfiguration<CategoryWithProduct>
    {
        public void Configure(EntityTypeBuilder<CategoryWithProduct> builder)
        {

            builder.HasKey(cp => new { cp.CategoryId, cp.ProductId });
        }
    }
}
