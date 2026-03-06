using ECommerce.Application.Common.Exceptions;
using ECommerce.Application.DTOs.Product;
using ECommerce.Application.Interfaces.Repositories;
using ECommerce.Application.Interfaces.Services;
using ECommerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<IEnumerable<ProductResponseDTO>> GetAllAsync()
        {
            var allProducts = await _productRepository.GetAllAsync();
            if (allProducts is null || !allProducts.Any())
            {
                throw new NotFoundException("Products not found.");
            }
            return allProducts.Select(MapToResponseDTO);
        }

        public async Task<ProductResponseDTO?> GetByIdAsync(Guid id)
        {
            var productById = await _productRepository.GetByIdAsync(id);
            return productById is null 
                ? throw new NotFoundException($"Product with {id} not found.") 
                : MapToResponseDTO(productById);
        }

        public async Task<ProductResponseDTO> CreateAsync(CreateProductDTO productCreateDTO)
        {
            var productToCreate = new Product(
                productCreateDTO.Name,
                productCreateDTO.Description,
                productCreateDTO.Image,
                productCreateDTO.Price,
                productCreateDTO.StockQuantity,
                productCreateDTO.DiscountPercentage,
                productCreateDTO.CategoryId,
                productCreateDTO.BrandId
            );

            await _productRepository.AddAsync(productToCreate);
            return MapToResponseDTO(productToCreate);

        }

        public async Task UpdateAsync(Guid id, UpdateProductDTO productUpdateDTO)
        {
            var productToUpdate = await _productRepository.GetByIdAsync(id) 
                ?? throw new NotFoundException($"Product with {id} not found.");


            productToUpdate.UpdateDetails(
                productUpdateDTO.Name,
                productUpdateDTO.Description,
                productUpdateDTO.Image,
                productUpdateDTO.Price
            );

            await _productRepository.UpdateAsync(productToUpdate);
        }

        public async Task DeleteAsync(Guid id)
        {
            var productToDelete = await _productRepository.GetByIdAsync(id) 
                ?? throw new NotFoundException($"Product with {id} not found.");

            productToDelete.SoftDelete();
            await _productRepository.UpdateAsync(productToDelete);
        }

        // Private helper method to map Product entity to ProductResponseDTO
        private ProductResponseDTO MapToResponseDTO(Product product) 
        {
            return new ProductResponseDTO
            {
                Id = product.ProductId,
                Name = product.ProductName,
                Image = product.ProductImage,
                Description = product.ProductDescription,
                Price = product.GetDiscountedPrice(),
                StockQuantity = product.StockQuantity,
                CategoryName = product.Category.CategoryName,
                BrandName = product.Brand.Name,
            };
        }
    }
}
