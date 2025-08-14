using Backend.Api.Dtos;
using Backend.Api.Models;

namespace Backend.Api.Services
{
    public interface ISupplierService
    {
        Task<IEnumerable<Supplier>> GetAllAsync();
        Task<Supplier> GetByIdAsync(int id);
        Task<Supplier> AddAsync(SupplierDtoIU dto);
        Task<Supplier?> UpdateAsync(int id, SupplierDtoIU dto);
        Task<bool> DeleteAsync(int id);
    }
}
