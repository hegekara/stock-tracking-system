using Backend.Api.Models;
using Backend.Api.DTOs;

namespace Backend.Api.Services
{
    public interface IOrderService
    {
        Task<IEnumerable<Order>> GetAllAsync();
        Task<Order?> GetByIdAsync(int id);
        Task<Order> AddAsync(OrderDtoIU orderDto);
        Task<Order?> UpdateAsync(int id, OrderDtoIU orderDto);
        Task<bool> DeleteAsync(int id);
    }
}
