using Backend.Api.Dtos;
using Backend.Api.Models;

namespace Backend.Api.Services
{
    public interface IStockTransactionService
    {
        Task<IEnumerable<StockTransaction>> GetAllAsync();
        Task<StockTransaction?> GetByIdAsync(int id);
        Task<StockTransaction> AddAsync(StockTransactionDtoIU dto);
        Task<StockTransaction?> UpdateAsync(int id, StockTransactionDtoIU dto);
        Task<bool> DeleteAsync(int id);
        Task<StockTransaction> AddStockAsync(StockTransactionDtoIU dto);
        Task<StockTransaction> RemoveStockAsync(StockTransactionDtoIU dto);
    }
}
