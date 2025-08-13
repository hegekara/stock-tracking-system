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

        public ProductService(
            IRepository<Product> productRepository,
            IRepository<Category> categoryRepository,
            IRepository<Supplier> supplierRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _supplierRepository = supplierRepository;
        }

        public async Task<IEnumerable<ProductDto>> GetAllAsync()
        {
            var products = await _productRepository.GetAllAsync(
                include: q => q.Include(p => p.Category)
                               .Include(p => p.Supplier)
            );

            return products.Where(p => p.Deleted == false || p.Deleted == null)
                .Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    UnitPrice = p.UnitPrice,
                    UnitsInStock = p.UnitsInStock,
                    CategoryId = p.Category.Id,
                    CategoryName = p.Category.Name,
                    SupplierId = p.Supplier.Id,
                    SupplierName = p.Supplier.Name
                });
        }

        public async Task<ProductDto?> GetByIdAsync(int id)
        {
            var product = await _productRepository.FirstOrDefaultAsync(
                p => p.Id == id && (p.Deleted == false || p.Deleted == null),
                include: q => q.Include(p => p.Category)
                               .Include(p => p.Supplier)
            );

            if (product == null) return null;

            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                UnitPrice = product.UnitPrice,
                UnitsInStock = product.UnitsInStock,
                CategoryId = product.Category.Id,
                CategoryName = product.Category.Name,
                SupplierId = product.Supplier.Id,
                SupplierName = product.Supplier.Name
            };
        }

        public async Task<ProductDto> CreateAsync(ProductDtoUI dto)
        {
            var category = await _categoryRepository.GetByIdAsync(dto.CategoryId)
                           ?? throw new Exception("Category not found");
            var supplier = await _supplierRepository.GetByIdAsync(dto.SupplierId)
                           ?? throw new Exception("Supplier not found");

            var product = new Product
            {
                Name = dto.Name,
                Description = dto.Description,
                UnitPrice = dto.UnitPrice,
                UnitsInStock = dto.UnitsInStock,
                Category = category,
                Supplier = supplier
            };

            await _productRepository.AddAsync(product);
            await _productRepository.SaveChangesAsync();

            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                UnitPrice = product.UnitPrice,
                UnitsInStock = product.UnitsInStock,
                CategoryId = category.Id,
                CategoryName = category.Name,
                SupplierId = supplier.Id,
                SupplierName = supplier.Name
            };
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

            product.Name = dto.Name;
            product.Description = dto.Description;
            product.UnitPrice = dto.UnitPrice;
            product.UnitsInStock = dto.UnitsInStock;
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
