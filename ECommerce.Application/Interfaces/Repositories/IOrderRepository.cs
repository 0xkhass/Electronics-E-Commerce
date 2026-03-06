using ECommerce.Domain.Entities;


namespace ECommerce.Application.Interfaces.Repositories
{
    public interface IOrderRepository
    {
        Task<Order?> GetOrderByIdAsync(Guid orderId);
        Task<IReadOnlyList<Order>> GetOrdersByUserIdAsync(Guid userId);
        Task AddOrderAsync(Order order);
        Task UpdateOrderAsync(Order order);
        Task DeleteOrderAsync(Guid orderId);
    }
}
