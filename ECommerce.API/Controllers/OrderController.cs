using ECommerce.Application.DTOs.Order;
using ECommerce.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }


        // GET: api/order/my-orders
        [HttpGet("my-orders")]
        public async Task<IActionResult> GetMyOrders() 
        {
            var userId = GetUserIdFromToken();
            var orders = await _orderService.GetAllOrdersByUserIdAsync(userId);
            return Ok(orders);
        }

        // GET: api/order/{orderId}
        [HttpGet("{orderId:guid}")]
        public async Task<IActionResult> GetOrderById(Guid orderId) 
        {
            var order = await _orderService.GetOrderByIdAsync(orderId);
            return Ok(order);
        }

        // POST: api/order
        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDTO createOrderDTO) 
        {
            // userId should come from the token, not from the request body
            createOrderDTO.UserId = GetUserIdFromToken();
            var createdOrder = await _orderService.CreateOrderAsync(createOrderDTO);
            return CreatedAtAction(nameof(GetOrderById), new { orderId = createdOrder.Id }, createdOrder);
        }

        // PUT api/order/{id}/status
        [HttpPut("{id:guid}/status")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] UpdateOrderStatusDTO updateOrderStatusDTO)
        {
            await _orderService.UpdateOrderStatusAsync(id, updateOrderStatusDTO);
            return NoContent();
        }

        // PUT api/order/{id}/cancel
        [HttpPut("{id:guid}/cancel")]
        public async Task<IActionResult> Cancel(Guid id)
        {
            await _orderService.CancelOrderAsync(id);
            return NoContent();
        }

        // Private helper
        private Guid GetUserIdFromToken()
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdClaim is null)
                throw new UnauthorizedAccessException("User ID not found in token.");
            return Guid.Parse(userIdClaim);
        }
    }
}
