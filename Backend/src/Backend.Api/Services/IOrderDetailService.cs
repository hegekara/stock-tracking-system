using Backend.Api.DTOs;

namespace Backend.Api.Services
{
    public interface IOrderDetailService
    {
        Task<IEnumerable<OrderDetailDto>> GetByOrderIdAsync(int orderId);
        Task<OrderDetailDto?> GetByIdAsync(int id);
        Task<OrderDetailDto> CreateAsync(int orderId, OrderDetailCreateDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
