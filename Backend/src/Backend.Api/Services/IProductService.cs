using Backend.Api.Dtos;

namespace Backend.Api.Services
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetAllAsync();
        Task<ProductDto?> GetByIdAsync(int id);
        Task<ProductDto> CreateAsync(ProductDtoUI dto);
        Task<bool> UpdateAsync(int id, ProductDtoUI dto);
        Task<bool> DeleteAsync(int id);
    }
}
