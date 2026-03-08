using ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Persistence.Configuration
{
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            // PK
            builder.HasKey(i => i.OrderItemId);

            // Properties
            builder.Property(o => o.UnitPrice)
                .HasPrecision(18, 2)
                .IsRequired();

            builder.Property(i => i.Quantity)
                .IsRequired();

            // Index on OrderId for
            builder.HasIndex(i => i.OrderId);

            // Relationships
            builder.HasOne(i => i.Product)
                .WithMany()
                .HasForeignKey(i => i.ProductId)
                .OnDelete(DeleteBehavior.Restrict); // don't delete items when product deleted.

        }
    }
}
