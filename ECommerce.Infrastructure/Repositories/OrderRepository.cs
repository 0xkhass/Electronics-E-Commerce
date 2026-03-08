using ECommerce.Application.Interfaces.Repositories;
using ECommerce.Domain.Entities;
using ECommerce.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;


namespace ECommerce.Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext _context;

        public OrderRepository(AppDbContext context)
        {
            _context = context;
        }
        // GET
        public async Task<Order?> GetOrderByIdAsync(Guid orderId)
        {
            return await _context.Orders
                    .Include(o => o.OrderItems)
                    .ThenInclude(i => i.Product)
                    .FirstOrDefaultAsync(o => o.OrderId == orderId);
        }

        public async Task<IReadOnlyList<Order>> GetAllOrdersByUserIdAsync(Guid userId)
        {
            return await _context.Orders.Where(o => o.UserId == userId).ToListAsync();
        }

        // POST

        public async Task AddOrderAsync(Order order)
        {
           await _context.Orders.AddAsync(order);
            //await _context.SaveChangesAsync();
        }
        // PUT
        public async Task UpdateOrderAsync(Order order)
        {
            _context.Orders.Update(order);
            //await _context.SaveChangesAsync();
        }

        public async Task CancelOrderAsync(Guid orderId) 
        {
            var orderToCancel = await _context.Orders.FindAsync(orderId);
            if (orderToCancel is not null)
            {
                orderToCancel.CancelOrder();
                _context.Orders.Update(orderToCancel);
                //await _context.SaveChangesAsync();
            }
        }

        // DELETE
        public async Task DeleteOrderAsync(Guid orderId)
        {
            var orderToDelete = await _context.Orders.FindAsync(orderId);
            if (orderToDelete is not null)
            {
                _context.Orders.Remove(orderToDelete);
                //await _context.SaveChangesAsync();
            }
        }
    }
}
