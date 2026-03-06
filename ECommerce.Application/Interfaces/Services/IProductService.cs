using ECommerce.Application.DTOs.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Interfaces.Services
{
    public interface IProductService
    {
        Task<ProductResponseDTO?> GetByIdAsync(Guid id);
        Task<IEnumerable<ProductResponseDTO>> GetAllAsync();
        Task<ProductResponseDTO> CreateAsync(CreateProductDTO productCreateDTO);
        Task UpdateAsync(Guid id, UpdateProductDTO productUpdateDTO);
        Task DeleteAsync(Guid id);

    }
}
