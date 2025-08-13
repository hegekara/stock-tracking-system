using Backend.Api.Dtos;

namespace Backend.Api.Services
{
    public interface IStockTransactionService
    {
        Task<IEnumerable<StockTransactionDto>> GetAllAsync();
        Task<StockTransactionDto?> GetByIdAsync(int id);
        Task<StockTransactionDto> AddAsync(StockTransactionDtoIU dto);
        Task<StockTransactionDto?> UpdateAsync(int id, StockTransactionDtoIU dto);
        Task<bool> DeleteAsync(int id);
    }
}
