using Backend.Api.Dtos;
using Backend.Api.Models;

namespace Backend.Api.Services
{
    public interface IStockTransactionService
    {
        Task<IEnumerable<StockTransaction>> GetAllAsync();
        Task<StockTransaction?> GetByIdAsync(int id);
        Task<StockTransaction> AddStockAsync(StockTransactionDtoIU dto);
        Task<StockTransaction> RemoveStockAsync(StockTransactionDtoIU dto);
    }
}
