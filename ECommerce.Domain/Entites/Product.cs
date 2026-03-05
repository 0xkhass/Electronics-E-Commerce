using ECommerce.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.Entites
{
    public class Product
    {
        public Guid ProductId { get; private set; }
        public string ProductName { get; private set; } = string.Empty;
        public string ProductDescription { get; private set; } = string.Empty;
        public string ProductImage { get; private set; } = string.Empty;
        public decimal ProductPrice { get; private set; }
        public int StockQuantity { get; private set; }
        public decimal ProductDiscountPercentage { get; private set; }
        public bool IsDeleted { get; private set; }
        public uint RowVersion { get; private set; } // For optimistic concurrency control
        public Guid BrandId { get; private set; }
        public Guid CategoryId { get; private set; }

        public Product() { } // For EF Core

        public Product(
            string name,
            string description,
            string image,
            decimal price,
            int stockQuantity,
            decimal discountPercentage,
            Guid categoryId,
            Guid brandId
        ) 
        {
            if (string.IsNullOrWhiteSpace(name)) 
            {
                throw new DomainExceptions("Product name is required.");
            }

            if (price <= 0) 
            {
                throw new DomainExceptions("Price must be greater than zero.");
            }

            if (stockQuantity < 0) 
            {
                throw new DomainExceptions("Stock quantity cannot be negative.");
            }

            if (discountPercentage < 0 || discountPercentage > 100) 
            {
                throw new DomainExceptions("Discount percentage must be between 0 and 100.");
            }

            ProductId = Guid.NewGuid();
            ProductName = name;
            ProductDescription = description;
            ProductImage = string.IsNullOrEmpty(image) ? string.Empty : image;
            ProductPrice = price;
            StockQuantity = stockQuantity;
            ProductDiscountPercentage = discountPercentage;
            CategoryId = categoryId;
            BrandId = brandId;
            IsDeleted = false;
        }

        public void DecreaseStock(int quantity) 
        {
            if (quantity <= 0) 
            {
                throw new DomainExceptions("Quantity must be greater than zero.");
            }
            if (quantity > StockQuantity) 
            {
                throw new DomainExceptions("Insufficient stock.");
            }
            StockQuantity -= quantity;
        }

        public void IncreaseStock(int quantity) 
        {
            if (quantity <= 0) 
            {
                throw new DomainExceptions("Quantity must be greater than zero.");
            }
            StockQuantity += quantity;
        }

        public void ApplyDiscount(decimal discountPercentage) 
        {
            if (discountPercentage < 0 || discountPercentage > 100) 
            {
                throw new DomainExceptions("Discount percentage must be between 0 and 100.");
            }
            ProductDiscountPercentage = discountPercentage;
        }

        public decimal GetDiscountedPrice() 
        {
            return ProductPrice * (1 - ProductDiscountPercentage / 100);
        }

        public void UpdateDetails(
            string name,
            string description,
            string image,
            decimal price
        )
        { 
            if (string.IsNullOrWhiteSpace(name)) 
            {
                throw new DomainExceptions("Product name is required.");
            }
            if (price <= 0) 
            {
                throw new DomainExceptions("Price must be greater than zero.");
            }
            ProductName = name;
            ProductDescription = description;
            ProductImage = string.IsNullOrEmpty(image) ? string.Empty : image;
            ProductPrice = price;
        }
        public void SoftDelete()
        {
            IsDeleted = true;
        }
    }
}
