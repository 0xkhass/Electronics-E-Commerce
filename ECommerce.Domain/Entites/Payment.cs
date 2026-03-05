using ECommerce.Domain.Enums;
using ECommerce.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.Entites
{
    
    public class Payment
    {
        public Guid PaymentId { get; private set; }
        public Guid OrderId { get; private set; } // FK
        public string TransactionId { get; private set; } = string.Empty; // Unique
        public PaymentMethod PaymentMethod { get; private set; }
        public PaymentStatus PaymentStatus { get; private set; }
        public DateTime CreatedAt { get; private set; }

        public Payment() { } // For EF Core

        public Payment(Guid orderId, PaymentMethod paymentMethod, string transactionId) 
        {
            if (orderId == Guid.Empty)
                throw new DomainExceptions("Order ID cannot be empty.");

            if (string.IsNullOrWhiteSpace(transactionId))
                throw new DomainExceptions("Transaction ID cannot be empty.");

            if (!Enum.IsDefined(typeof(PaymentMethod), paymentMethod))
                throw new DomainExceptions("Invalid payment method.");

            PaymentId = Guid.NewGuid();
            OrderId = orderId;
            PaymentMethod = paymentMethod;
            TransactionId = transactionId;
            PaymentStatus = PaymentStatus.Pending;
            CreatedAt = DateTime.UtcNow;
        }

        public void MarkAsCompleted() 
        {
            if (PaymentStatus != PaymentStatus.Pending)
                throw new DomainExceptions("Only pending payments can be marked as completed.");
            PaymentStatus = PaymentStatus.Completed;
        }

        public void MarkAsFailed() 
        {
            if (PaymentStatus != PaymentStatus.Pending)
                throw new DomainExceptions("Only pending payments can be marked as failed.");
            PaymentStatus = PaymentStatus.Failed;
        }

        public void Refund() 
        {
            if (PaymentStatus != PaymentStatus.Completed)
                throw new DomainExceptions("Only completed payments can be refunded.");
            PaymentStatus = PaymentStatus.Refunded;
        }

    }
}
