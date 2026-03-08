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
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            // PK
            builder.HasKey(o => o.OrderId);


            // Properties
            builder.Property(o => o.TotalAmount)
                .HasPrecision(18, 2)
                .IsRequired();

            builder.Property(o => o.Status)
                .HasConversion<string>() // store as string not int
                .IsRequired();

            builder.Property(o => o.OrderDate)
                .IsRequired();

            builder.Property(o => o.CreatedAt)
                .IsRequired();

            builder.Property(o => o.UpdatedAt)
                .IsRequired();


            // Relationships
            builder.HasOne(o => o.User)
                .WithMany()
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Restrict); // Don't delete order when user deleted.

            builder.HasOne(o => o.Payment)
                .WithOne(p => p.Order)
                .HasForeignKey<Order>(o => o.PaymentId) // Order is dependent on Payment
                .IsRequired(false) // Payment id is nullable
                .OnDelete(DeleteBehavior.SetNull); 

            builder.HasMany(o => o.OrderItems)
                .WithOne(i => i.Order)
                .HasForeignKey(i => i.OrderId)
                .OnDelete(DeleteBehavior.Cascade); // Delete order items when order deleted

        }
    }
}
