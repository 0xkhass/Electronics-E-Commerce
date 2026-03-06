using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Application.Interfaces.Repositories;
using ECommerce.Domain.Entities;
using ECommerce.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;

        public ProductRepository(AppDbContext context) 
        {
            _context = context;
        }
        // CRUD operations for Product entity
        // GET Methods

        /// <summary>
        /// Retrieves a product by its unique identifier, 
        /// including its associated category and brand information.
        /// </summary>
        /// <returns>The product entity if found; otherwise, null.</returns>

        public async Task<Product?> GetByIdAsync(Guid id)
        {
            return await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .FirstOrDefaultAsync(p => p.ProductId == id);
        }
        /// <summary>
        /// Retrieves all products from the database, 
        /// including their associated category and brand information. 
        /// This method returns a list of products that can be used for display or 
        /// further processing in the application.   
        /// </summary>
        /// <returns>The Products entity if found; otherwise, null.</returns>
        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .ToListAsync();
        }

        // POST Methods
        /// <summary>
        /// Create a new product in the database. 
        /// This method takes a Product entity as input, 
        /// adds it to the database context, and saves the changes to persist the new product 
        /// in the database.
        /// </summary>
        /// <param name="product">The product entity to create.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task AddAsync(Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Asynchronously updates the specified product in the data store.
        /// </summary>
        /// <param name="product">The product entity to update.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        // PUT Methods
        public async Task UpdateAsync(Product product)
        {

            _context.Products.Update(product); // no await why ?
            await _context.SaveChangesAsync();
        }


        // DELETE Methods
        /// <summary>
        /// Asynchronously deletes the product with the specified identifier from the data store.
        /// </summary>
        /// <param name="id">The unique identifier of the product to delete.</param>
        /// <returns>A task representing the asynchronous delete operation.</returns>
        public async Task DeleteAsync(Guid id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null) 
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
        }

        
    }
}
