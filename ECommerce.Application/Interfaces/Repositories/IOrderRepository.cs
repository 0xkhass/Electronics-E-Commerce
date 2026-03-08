using ECommerce.Domain.Entities;


namespace ECommerce.Application.Interfaces.Repositories
{
    public interface IOrderRepository
    {
        Task<Order?> GetOrderByIdAsync(Guid orderId);
        Task<IReadOnlyList<Order>> GetAllOrdersByUserIdAsync(Guid userId);
        Task AddOrderAsync(Order order);
        Task UpdateOrderAsync(Order order);
        Task CancelOrderAsync(Guid orderId);
        Task DeleteOrderAsync(Guid orderId);
    }
}
