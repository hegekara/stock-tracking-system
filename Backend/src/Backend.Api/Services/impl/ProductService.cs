using AutoMapper;
using Backend.Api.Dtos;
using Backend.Api.Models;
using Backend.Api.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Backend.Api.Services
{
    public class ProductService : IProductService
    {
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<Category> _categoryRepository;
        private readonly IRepository<Supplier> _supplierRepository;
        private readonly IMapper _mapper;

        public ProductService(
            IRepository<Product> productRepository,
            IRepository<Category> categoryRepository,
            IRepository<Supplier> supplierRepository,
            IMapper mapper)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _supplierRepository = supplierRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            var products = await _productRepository.GetAllAsync(
                include: q => q.Include(p => p.Category)
                               .Include(p => p.Supplier)
                               .ThenInclude(x => x.Address)
            );

            var activeProducts = products
                .Where(p => p.Deleted == false || p.Deleted == null);

            return activeProducts;
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            var product = await _productRepository.FirstOrDefaultAsync(
                p => p.Id == id && (p.Deleted == false || p.Deleted == null),
                include: q => q.Include(p => p.Category)
                               .Include(p => p.Supplier)
                               .ThenInclude(x => x.Address)
            );

            return product;
        }

        public async Task<Product> CreateAsync(ProductDtoUI dto)
        {
            var category = await _categoryRepository.GetByIdAsync(dto.CategoryId)
                           ?? throw new Exception("Category not found");

            var supplier = await _supplierRepository.GetByIdAsync(dto.SupplierId)
                           ?? throw new Exception("Supplier not found");

            var product = _mapper.Map<Product>(dto);
            product.Category = category;
            product.Supplier = supplier;

            await _productRepository.AddAsync(product);
            await _productRepository.SaveChangesAsync();

            return product;
        }

        public async Task<bool> UpdateAsync(int id, ProductDtoUI dto)
        {
            var product = await _productRepository.FirstOrDefaultAsync(
                p => p.Id == id && (p.Deleted == false || p.Deleted == null),
                include: q => q.Include(p => p.Category)
                               .Include(p => p.Supplier)
            );

            if (product == null) return false;

            var category = await _categoryRepository.GetByIdAsync(dto.CategoryId)
                           ?? throw new Exception("Category not found");

            var supplier = await _supplierRepository.GetByIdAsync(dto.SupplierId)
                           ?? throw new Exception("Supplier not found");

            _mapper.Map(dto, product);
            product.Category = category;
            product.Supplier = supplier;

            _productRepository.Update(product);
            await _productRepository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null) return false;

            product.Deleted = true;
            _productRepository.Update(product);
            await _productRepository.SaveChangesAsync();
            return true;
        }
    }
}
