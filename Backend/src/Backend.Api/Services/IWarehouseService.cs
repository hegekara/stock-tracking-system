using Backend.Api.Dtos;
using Backend.Api.Models;

namespace Backend.Api.Services
{
    public interface IWarehouseService
    {
        Task<IEnumerable<Warehouse>> GetAllAsync();
        Task<Warehouse?> GetByIdAsync(int id);
        Task<Warehouse> CreateAsync(WarehouseDtoIU dto);
        Task<Warehouse?> UpdateAsync(int id, WarehouseDtoIU dto);
        Task<bool> DeleteAsync(int id);
    }
}