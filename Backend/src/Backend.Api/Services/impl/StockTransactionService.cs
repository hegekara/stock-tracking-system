using AutoMapper;
using Backend.Api.Dtos;
using Backend.Api.Models;
using Backend.Api.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Backend.Api.Services
{
    public class StockTransactionService : IStockTransactionService
    {
        private readonly IRepository<StockTransaction> _transactionRepository;
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<Warehouse> _warehouseRepository;
        private readonly IMapper _mapper;

        public StockTransactionService(
            IRepository<StockTransaction> transactionRepository,
            IRepository<Product> productRepository,
            IRepository<Warehouse> warehouseRepository,
            IMapper mapper)
        {
            _transactionRepository = transactionRepository;
            _productRepository = productRepository;
            _warehouseRepository = warehouseRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<StockTransactionDto>> GetAllAsync()
        {
            var transactions = await _transactionRepository
                .FindAsync(t => t.Deleted == false || t.Deleted == null, include: q => 
                    q.Include(x => x.Product)
                     .Include(x => x.Warehouse));

            return _mapper.Map<IEnumerable<StockTransactionDto>>(transactions);
        }

        public async Task<StockTransactionDto?> GetByIdAsync(int id)
        {
            var transaction = await _transactionRepository
                .FirstOrDefaultAsync(t => t.Id == id && (t.Deleted == false || t.Deleted == null), include: q =>
                    q.Include(x => x.Product)
                     .Include(x => x.Warehouse));

            return transaction == null ? null : _mapper.Map<StockTransactionDto>(transaction);
        }

        public async Task<StockTransactionDto> AddAsync(StockTransactionDtoIU dto)
        {
            var transaction = _mapper.Map<StockTransaction>(dto);

            transaction.Product = await _productRepository.GetByIdAsync(dto.ProductId)
                ?? throw new Exception("Product not found");
            transaction.Warehouse = await _warehouseRepository.GetByIdAsync(dto.WarehouseId)
                ?? throw new Exception("Warehouse not found");

            await _transactionRepository.AddAsync(transaction);
            await _transactionRepository.SaveChangesAsync();

            return _mapper.Map<StockTransactionDto>(transaction);
        }

        public async Task<StockTransactionDto?> UpdateAsync(int id, StockTransactionDtoIU dto)
        {
            var existing = await _transactionRepository.GetByIdAsync(id);
            if (existing == null || existing.Deleted == true)
                return null;

            _mapper.Map(dto, existing);
            existing.Product = await _productRepository.GetByIdAsync(dto.ProductId)
                ?? throw new Exception("Product not found");
            existing.Warehouse = await _warehouseRepository.GetByIdAsync(dto.WarehouseId)
                ?? throw new Exception("Warehouse not found");

            _transactionRepository.Update(existing);
            await _transactionRepository.SaveChangesAsync();

            return _mapper.Map<StockTransactionDto>(existing);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var transaction = await _transactionRepository.GetByIdAsync(id);
            if (transaction == null || transaction.Deleted == true)
                return false;

            transaction.Deleted = true;
            _transactionRepository.Update(transaction);
            await _transactionRepository.SaveChangesAsync();
            return true;
        }
    }
}
