using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.DTOs.Order
{
    public class OrderResponseDTO
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = string.Empty; // string, not enum
        public DateTime OrderDate { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public Guid? PaymentId { get; set; } // Nullable
        public required List<OrderItemResponseDTO> Items { get; set; }
    }
}
