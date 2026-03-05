using ECommerce.Domain.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ECommerce.Application.Interfaces
{
    public interface IProductRepository
    {
        // CRUD operations for Product entity
        Task<Product?> GetByIdAsync(Guid id); // Retrieve a product by its unique identifier
        Task<List<Product>> GetAllAsync(); // Retrieve all products
        Task AddAsync(Product product); // Add a new product to the repository
        Task UpdateAsync(Product product); // Update an existing product's details
        Task DeleteAsync(Guid id); // Remove a product from the repository by its unique identifier

    }
}
