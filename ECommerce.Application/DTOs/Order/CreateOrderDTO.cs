using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.DTOs.Order
{
    public class CreateOrderDTO
    {
        public Guid UserId { get; set; }
        public required List<CreateOrderItemDTO> Items { get; set; }

    }
}
