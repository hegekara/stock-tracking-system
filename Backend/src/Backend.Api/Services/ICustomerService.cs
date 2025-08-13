using Backend.Api.Models;

namespace Backend.Api.Services
{
    public interface ICustomerService
    {
        Task<IEnumerable<Customer>> GetAllAsync();
        Task<Customer?> GetByIdAsync(int id);
        Task<Customer> AddAsync(CustomerDtoIU customer);
        Task<Customer?> UpdateAsync(int id, CustomerDtoIU customer);
        Task<bool> DeleteAsync(int id);
    }
}
