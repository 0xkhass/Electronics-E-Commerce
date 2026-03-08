using ECommerce.Application.DTOs.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Interfaces.Services
{
    public interface IOrderService
    {
        Task<OrderResponseDTO?> GetOrderByIdAsync(Guid orderId);
        Task<IEnumerable<OrderResponseDTO>> GetAllOrdersByUserIdAsync(Guid userId);
        Task<OrderResponseDTO> CreateOrderAsync(CreateOrderDTO createOrderDTO);
        Task UpdateOrderStatusAsync(Guid orderId, UpdateOrderStatusDTO updateOrderDTO);

        Task CancelOrderAsync(Guid orderId);
        Task DeleteOrderStatusAsync(Guid orderId);
    }
}
