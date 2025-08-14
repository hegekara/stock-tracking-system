using Backend.Api.Dtos;
using Backend.Api.Models;

namespace Backend.Api.Services
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product?> GetByIdAsync(int id);
        Task<Product> CreateAsync(ProductDtoUI dto);
        Task<bool> UpdateAsync(int id, ProductDtoUI dto);
        Task<bool> DeleteAsync(int id);
    }
}
