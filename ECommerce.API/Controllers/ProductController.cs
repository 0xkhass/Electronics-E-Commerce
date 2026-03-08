using ECommerce.Application.DTOs.Product;
using ECommerce.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        // GET: api/Product
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllProduct() 
        {
            var products = await _productService.GetAllAsync();
            return Ok(products);
        }

        // GET: api/Product/{id}
        [HttpGet("{productId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetProductById(Guid productId) 
        {
            var product = await _productService.GetByIdAsync(productId);
            return Ok(product);
        }

        // POST: api/Product
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductDTO createProductDTO) 
        {
            var created = await _productService.CreateAsync(createProductDTO);
            return CreatedAtAction(nameof(GetProductById), new { productId = created.Id }, created);
        }

        // PUT: api/Product/{id}
        [HttpPut("{productId:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateProduct(Guid productId, [FromBody] UpdateProductDTO updateProductDTO) 
        {
            await _productService.UpdateAsync(productId, updateProductDTO);
            return NoContent();
        }


        // DELETE: api/Product/{id}
        [HttpDelete("{productId:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteProduct(Guid productId) 
        {
            await _productService.DeleteAsync(productId);
            return NoContent();
        }

    }
}
