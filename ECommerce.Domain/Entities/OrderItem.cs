using ECommerce.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.Entities
{
    public class OrderItem
    {
        public Guid OrderItemId { get; private set; }
        public Guid OrderId { get; private set; } // FK, Index
        public Order Order { get; private set; } = null!;// Navigation property

        public Guid ProductId { get; private set; }// FK
        public Product Product { get; private set; } = null!;// Navigation property
        public int Quantity { get; private set; }
        public decimal UnitPrice { get; private set; }

        public OrderItem() { } // For EF Core

        public OrderItem(Guid orderId, Guid productId, int quantity, decimal unitPrice) 
        {
            if (orderId == Guid.Empty)
                throw new DomainExceptions("Order ID cannot be empty.");

            if (productId == Guid.Empty)
                throw new DomainExceptions("Product ID cannot be empty.");

            if (quantity <= 0)
                throw new DomainExceptions("Quantity must be greater than zero.");

            if (unitPrice < 0)
                throw new DomainExceptions("Unit price cannot be negative.");

            OrderItemId = Guid.NewGuid();
            OrderId = orderId;
            ProductId = productId;
            Quantity = quantity;
            UnitPrice = unitPrice;
        }

        public void IncreaseQuantity(int amount) 
        {
            if (amount <= 0)
                throw new DomainExceptions("Amount to increase must be greater than zero.");

            Quantity += amount;
        }

        public void UpdateQuantity(int newQuantity) 
        { 
            if (newQuantity <= 0)
                throw new DomainExceptions("Quantity must be greater than zero.");

            Quantity = newQuantity;
        }

        public decimal GetSubTotal() => Quantity * UnitPrice;
    }
}
