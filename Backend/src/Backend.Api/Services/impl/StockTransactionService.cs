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
        private readonly IMapper _mapper;

        public StockTransactionService(
            IRepository<StockTransaction> transactionRepository,
            IRepository<Product> productRepository,
            IMapper mapper)
        {
            _transactionRepository = transactionRepository;
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<StockTransaction>> GetAllAsync()
        {
            var transactions = await _transactionRepository
                .FindAsync(
                    t => t.Deleted == false || t.Deleted == null,
                    include: q => q
                        .Include(x => x.Product)
                            .ThenInclude(x => x.Category)
                        .Include(x => x.Product)
                            .ThenInclude(x => x.Supplier)
                                .ThenInclude(x => x.Address)
                        .OrderByDescending(d => d.TransactionDate)
                );

            return transactions;
        }

        public async Task<StockTransaction?> GetByIdAsync(int id)
        {
            var transaction = await _transactionRepository
                .FirstOrDefaultAsync(t => t.Id == id && (t.Deleted == false || t.Deleted == null), include: q =>
                    q.Include(x => x.Product));

            return transaction == null ? null : transaction;
        }

        public async Task<StockTransaction> AddStockAsync(StockTransactionDtoIU dto)
        {
            var product = await _productRepository.FirstOrDefaultAsync(
                p => p.Id == dto.ProductId && (p.Deleted == false || p.Deleted == null),
                include: q => q.Include(p => p.Category).Include(p => p.Supplier));

            if (product == null)
                throw new Exception($"Product with ID {dto.ProductId} not found");

            product.UnitsInStock += dto.Quantity;

            var transaction = _mapper.Map<StockTransaction>(dto);
            transaction.TransactionType = StockTransactionType.Inbound;
            transaction.Product = product;

            await _transactionRepository.AddAsync(transaction);
            _productRepository.Update(product);

            await _transactionRepository.SaveChangesAsync();

            return transaction;
        }

        public async Task<StockTransaction> RemoveStockAsync(StockTransactionDtoIU dto)
        {
            var product = await _productRepository.FirstOrDefaultAsync(
                p => p.Id == dto.ProductId && (p.Deleted == false || p.Deleted == null),
                include: q => q.Include(p => p.Category).Include(p => p.Supplier));

            if (product == null)
                throw new Exception($"Product with ID {dto.ProductId} not found");

            if (product.UnitsInStock < dto.Quantity)
                throw new Exception("Not enough stock to complete the transaction");

            product.UnitsInStock -= dto.Quantity;

            var transaction = _mapper.Map<StockTransaction>(dto);
            transaction.TransactionType = StockTransactionType.Outbound;
            transaction.Product = product;

            await _transactionRepository.AddAsync(transaction);
            _productRepository.Update(product);

            await _transactionRepository.SaveChangesAsync();

            return transaction;
        }
    }
}
