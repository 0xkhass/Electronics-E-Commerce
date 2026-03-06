using ECommerce.Domain.Enums;
using ECommerce.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.Entities
{
    public class Order
    {
        public Guid OrderId { get; private set; }
        public Guid UserId { get; private set; }
        public User User { get; private set; } = null!; // Navigation property
        public Guid PaymentId { get; private set; }
        public Payment Payment { get; private set; } = null!; // Navigation property
        public decimal TotalAmount { get; private set; }
        public OrderStatus Status { get; private set; }
        private readonly List<OrderItem> _orderItems = new();
        public IReadOnlyCollection<OrderItem> OrderItems => _orderItems.AsReadOnly();

        public DateTime OrderDate { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }


        public Order() { } // For EF Core

        public Order(Guid userId) 
        {
            if (userId == Guid.Empty)
                throw new DomainExceptions("User ID cannot be empty.");

            OrderId = Guid.NewGuid();
            UserId = userId;
            Status = OrderStatus.Pending;
            TotalAmount = 0;
            OrderDate = DateTime.UtcNow;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        public void AddOrderItem(Guid productId, int quantity, decimal unitPrice) 
        {
            if (Status != OrderStatus.Pending)
                throw new DomainExceptions("Cannot modify an order that is not longer pending.");

            var existingItem = _orderItems.FirstOrDefault(i => i.ProductId == productId);
            if (existingItem != null)
            {
                existingItem.IncreaseQuantity(quantity);
            }
            else 
            {
                var item = new OrderItem(OrderId, productId, quantity, unitPrice);
                _orderItems.Add(item);
            }

            RecalculateTotal();
            UpdatedAt = DateTime.UtcNow;
        }

        public void RemoveOrderItem(Guid productId) 
        { 
            if (Status != OrderStatus.Pending)
                throw new DomainExceptions("Cannot modify an order that is not longer pending.");

            var item = _orderItems.FirstOrDefault(i => i.ProductId == productId);
            if (item is null)
                throw new DomainExceptions("Order item not found.");

            _orderItems.Remove(item);
            RecalculateTotal();
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateStatus(OrderStatus newStatus) 
        {
            // BASIC status transition rules
            if (Status == OrderStatus.Cancelled || Status == OrderStatus.Expired)
                throw new DomainExceptions("Cannot change status of a cancelled or expired order.");

            if (Status == OrderStatus.Shipped)
                throw new DomainExceptions("Cannot change status of a shipped order.");

            Status = newStatus;
            UpdatedAt = DateTime.UtcNow;
        }

        public void CancelOrder() 
        {
            if (Status == OrderStatus.Shipped)
                throw new DomainExceptions("Cannot cancel a shipped order.");

            if (Status == OrderStatus.Cancelled)
                throw new DomainExceptions("Order is already cancelled.");

            Status = OrderStatus.Cancelled;
            UpdatedAt = DateTime.UtcNow;
        }
        private void RecalculateTotal() 
        {
            TotalAmount = _orderItems.Sum(i => i.Quantity * i.UnitPrice);
        }
    }
}
