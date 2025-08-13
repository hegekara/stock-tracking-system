using Backend.Api.Models;

namespace Backend.Api.Services
{
    public interface IAddressService
    {
        Task<IEnumerable<Address>> GetAllAsync();
        Task<Address?> GetByIdAsync(int id);
        Task<Address> CreateAsync(Address address);
        Task<Address?> UpdateAsync(int id, Address address);
        Task<bool> DeleteAsync(int id);
    }
}
