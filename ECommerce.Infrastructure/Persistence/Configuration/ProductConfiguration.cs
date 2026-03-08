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
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            // PK
            builder.HasKey(p => p.ProductId);

            // Properties
            builder.Property(p => p.ProductName)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(p => p.ProductDescription)
                .HasMaxLength(2000);

            builder.Property(p => p.ProductImage)
                .HasMaxLength(500);

            builder.Property(p => p.ProductPrice)
                .HasPrecision(18, 2)
                .IsRequired();

            builder.Property(p => p.ProductDiscountPercentage)
                .HasPrecision(5, 2);

            builder.Property(p => p.StockQuantity)
                .IsRequired();

            builder.Property(p => p.IsDeleted)
                .IsRequired()
                .HasDefaultValue(false);

            // RowVersion for optimistic concurrency
            builder.Property(p => p.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();

            // Index on Product name
            builder.HasIndex(p => p.ProductName);

            // Relationships
            builder.HasOne(p => p.Brand)
                .WithMany()
                .HasForeignKey(p => p.BrandId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.Category)
                .WithMany()
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
