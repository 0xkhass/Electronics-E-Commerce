using ECommerce.Application.Interfaces.Repositories;
using ECommerce.Domain.Entities;
using ECommerce.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;


namespace ECommerce.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        // CRUD operations for User entity
        // GET
        /// <summary>
        /// Retrieves a user by email address asynchronously.
        /// </summary>
        /// <remarks>Throws an exception if no user is found with the given email address.</remarks>
        /// <param name="email">The email address of the user to retrieve.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the user with the specified
        /// email address.</returns>
        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _context.Users
                .FirstAsync(u => u.Email == email); // This will throw an exception if no user is found with the given email.
            // You can handle this in by using FirstOrDefaultAsync to return null instead.
            // FirstOrDefaultAsync(u => u.Email == email); 
            // Use this if you want to return null instead of throwing an exception when no user is found

        }

        /// <summary>
        /// Retrieves a user by their unique identifier asynchronously.
        /// </summary>
        /// <remarks>Throws an exception if no user is found with the given identifier.</remarks>
        /// <param name="userId">The unique identifier of the user to retrieve.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the user with the specified
        /// identifier.</returns>
        public async Task<User> GetUserByIdAsync(Guid userId)
        {
            return await _context.Users
                .FirstAsync(u => u.UserId == userId); // This will throw an exception if no user is found with the given userId.
            // Same as above, you can handle this by using FirstOrDefaultAsync to return null instead.
        }

        // POST
        /// <summary>
        /// Adds a user to the database asynchronously.
        /// </summary>
        /// <param name="user">The user entity to add.</param>
        /// <returns>A task representing the asynchronous operation.</returns>

        public async Task AddUserAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        // PUT
        /// <summary>
        /// Updates the specified user in the database asynchronously.
        /// </summary>
        /// <param name="user">The user entity to update.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task UpdateUserAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }


        // DELETE
        /// <summary>
        /// Deletes the user with the specified identifier from the data store asynchronously.
        /// </summary>
        /// <param name="userId">The unique identifier of the user to delete.</param>
        /// <returns>A task representing the asynchronous delete operation.</returns>
        public async Task DeleteUserAsync(Guid userId)
        {
            var userToDelete = await _context.Users.FindAsync(userId);
                
            if (userToDelete != null) 
            {
                _context.Users.Remove(userToDelete);
                await _context.SaveChangesAsync();
            }
        }
    }
}
