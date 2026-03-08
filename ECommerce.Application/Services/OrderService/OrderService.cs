using ECommerce.Application.Common.Exceptions;
using ECommerce.Application.DTOs.Order;
using ECommerce.Application.Interfaces;
using ECommerce.Application.Interfaces.Repositories;
using ECommerce.Application.Interfaces.Services;
using ECommerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Services.OrderService
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IUserRepository _userRepository;
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;

        public OrderService(IOrderRepository orderRepository, 
            IUserRepository userRepository, IProductRepository productRepository, 
            IUnitOfWork unitOfWork)
        {
            _orderRepository = orderRepository;
            _userRepository = userRepository;
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
        }


        // GET ALL ORDERS
        public async Task<IEnumerable<OrderResponseDTO>> GetAllOrdersByUserIdAsync(Guid userId)
        {
            if (userId == Guid.Empty)
            {
                throw new ValidationException("User ID cannot be empty.");
            }
            var users = await _userRepository.GetUserByIdAsync(userId) 
                ?? throw new NotFoundException($"User with ID {userId} not found.");

            var orders = await _orderRepository.GetAllOrdersByUserIdAsync(userId);

            return orders.Select(MapToResponseDTO);
        }

        public async Task<OrderResponseDTO?> GetOrderByIdAsync(Guid orderId)
        {
           if (orderId == Guid.Empty)
                throw new ValidationException("Order ID cannot be empty.");

           var order = await _orderRepository.GetOrderByIdAsync(orderId);
            return order is null 
                ? throw new NotFoundException($"Order with ID {orderId} not found.") 
                : MapToResponseDTO(order);
        }

        public async Task<OrderResponseDTO> CreateOrderAsync(CreateOrderDTO createOrderDTO)
        {
            // Validate user
            _ = await _userRepository.GetUserByIdAsync(createOrderDTO.UserId)
                ?? throw new NotFoundException($"User with ID {createOrderDTO.UserId} not found.");
            // Validate items list is not empty
            if (createOrderDTO.Items == null || !createOrderDTO.Items.Any())
                throw new ValidationException("Order must contain at least one item.");

            // Create order aggregate
            var order = new Order(createOrderDTO.UserId);

            // Process each item
            foreach (var item in createOrderDTO.Items)
            {
                var product = await _productRepository.GetByIdAsync(item.ProductId);

                if (product is null || product.IsDeleted)
                    throw new NotFoundException($"Product with ID {item.ProductId} not found.");

                if (product.StockQuantity < item.Quantity)
                    throw new ValidationException($"Insufficient stock for product {product.ProductName}. " +
                        $"Requested: {item.Quantity}, Available: {product.StockQuantity}.");

                // Look in disocunted price at time of order
                var priceAtOrder = product.GetDiscountedPrice();

                // add to order + decrease stock
                order.AddOrderItem(item.ProductId, item.Quantity, priceAtOrder);
                product.DecreaseStock(item.Quantity);

                await _productRepository.UpdateAsync(product);
            }

            // Persist and save atomically
            await _orderRepository.AddOrderAsync(order);
            await _unitOfWork.SaveChangesAsync();

            return MapToResponseDTO(order);
        }

        public async Task UpdateOrderStatusAsync(Guid orderId, UpdateOrderStatusDTO updateOrderDTO)
        {
            if (orderId == Guid.Empty) 
                throw new ValidationException("Order ID cannot be empty.");

            var order = await _orderRepository.GetOrderByIdAsync(orderId) 
                ?? throw new NotFoundException($"Order with ID {orderId} not found.");
            order.UpdateStatus(updateOrderDTO.Status);

            await _orderRepository.UpdateOrderAsync(order);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task CancelOrderAsync(Guid orderId)
        {
            if (orderId == Guid.Empty)
                throw new ValidationException("Order ID cannot be empty.");

            var order = await _orderRepository.GetOrderByIdAsync(orderId) 
                ?? throw new NotFoundException($"Order with ID {orderId} not found.");

            // Restore stock for each item
            foreach (var item in order.OrderItems)
            {
                var product = await _productRepository.GetByIdAsync(item.ProductId);
                if (product != null)
                {
                    product.IncreaseStock(item.Quantity);
                    await _productRepository.UpdateAsync(product);
                }
            }

            order.CancelOrder();
            await _orderRepository.UpdateOrderAsync(order);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteOrderStatusAsync(Guid orderId)
        {
            if (orderId == Guid.Empty)
                throw new ValidationException("Order ID cannot be empty.");

            var orderToDelete = await _orderRepository.GetOrderByIdAsync(orderId) 
                ?? throw new NotFoundException($"Order with ID {orderId} not found.");

            await _orderRepository.DeleteOrderAsync(orderToDelete.OrderId);
            await _unitOfWork.SaveChangesAsync();
        }


        private OrderResponseDTO MapToResponseDTO(Order order) 
        {
            return new OrderResponseDTO 
            {
                Id = order.OrderId,
                UserId = order.UserId,
                TotalAmount = order.TotalAmount,
                Status = order.Status.ToString(),
                OrderDate = order.OrderDate,
                CreatedAt = order.CreatedAt,
                UpdatedAt = order.UpdatedAt,
                PaymentId = order.PaymentId,
                Items = [.. order.OrderItems.Select(oi => new OrderItemResponseDTO 
                { 
                    OrderItemId = oi.OrderItemId,
                    ProductId = oi.ProductId,
                    ProductName = oi.Product?.ProductName ?? string.Empty,
                    Quantity = oi.Quantity,
                    UnitPrice = oi.UnitPrice,
                    SubTotal = oi.GetSubTotal()
                })]
            };
        }
    }
}
