using ECommerce.Domain.Enums;
using ECommerce.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.Entities
{
    public class Order
    {
        public Guid OrderId { get; private set; }
        public Guid UserId { get; private set; }
        public User User { get; private set; } = null!; // Navigation property
        public Guid? PaymentId { get; private set; }
        public Payment? Payment { get; private set; } = null!; // Navigation property
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

            if (productId == Guid.Empty)
                throw new DomainExceptions("Product ID cannot be empty.");
            if (quantity <= 0)
                throw new DomainExceptions("Quantity must be greater than zero.");
            if (unitPrice < 0)
                throw new DomainExceptions("Unit price cannot be negative.");

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

        public void AssignPayment(Guid paymentId) 
        {
            if (paymentId == Guid.Empty)
                throw new DomainExceptions("Payment ID cannot be empty.");
            if (PaymentId.HasValue)
                throw new DomainExceptions("Payment is already assigned to this order.");

            PaymentId = paymentId;
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

        private static readonly Dictionary<OrderStatus, OrderStatus[]> _allowedTransitions = new()
        {
            // Pending => Paid => Shipped => Cancelled/Expired
            { OrderStatus.Pending, [OrderStatus.Cancelled]},
            { OrderStatus.Paid, [OrderStatus.Shipped, OrderStatus.Cancelled]},
            { OrderStatus.Shipped, [OrderStatus.Shipped]},
            { OrderStatus.Cancelled, []},
            { OrderStatus.Expired, []},
        };

        public void UpdateStatus(OrderStatus newStatus)
        {
            if (!_allowedTransitions[Status].Contains(newStatus)) 
                throw new DomainExceptions($"Invalid status transition from {Status} to {newStatus}.");
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
