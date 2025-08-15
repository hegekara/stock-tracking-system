using Xunit;
using Moq;
using FluentAssertions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Api.Models;
using Backend.Api.Repositories;
using Backend.Api.Services;
using Backend.Api.Dtos;
using AutoMapper;

namespace Backend.Api.Tests.Services
{
    public class ProductServiceTests
    {
        private readonly Mock<IRepository<Product>> _productRepoMock;
        private readonly Mock<IRepository<Category>> _categoryRepoMock;
        private readonly Mock<IRepository<Supplier>> _supplierRepoMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly ProductService _service;

        public ProductServiceTests()
        {
            _productRepoMock = new Mock<IRepository<Product>>();
            _categoryRepoMock = new Mock<IRepository<Category>>();
            _supplierRepoMock = new Mock<IRepository<Supplier>>();
            _mapperMock = new Mock<IMapper>();

            _service = new ProductService(
                _productRepoMock.Object,
                _categoryRepoMock.Object,
                _supplierRepoMock.Object,
                _mapperMock.Object
            );
        }

        [Fact]
        public async Task GetAllAsync_ReturnProducts()
        {
            var products = new List<Product>
            {
                new Product { Id = 1, Name = "product a", Deleted = false, Category = new Category(), Supplier = new Supplier() },
                new Product { Id = 2, Name = "product b", Deleted = null, Category = new Category(), Supplier = new Supplier() },
                new Product { Id = 3, Name = "product c", Deleted = true, Category = new Category(), Supplier = new Supplier() }
            };

            _productRepoMock.Setup(r => r.GetAllAsync(It.IsAny<Func<IQueryable<Product>, IQueryable<Product>>>()))
                .ReturnsAsync(products);

            var result = await _service.GetAllAsync();

            result.Should().HaveCount(2);
            result.Should().OnlyContain(p => p.Deleted == false || p.Deleted == null);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnProduct()
        {
            var product = new Product { Id = 1, Name = "product a", Deleted = false, Category = new Category(), Supplier = new Supplier() };

            _productRepoMock.Setup(r => r.FirstOrDefaultAsync(
                It.IsAny<System.Linq.Expressions.Expression<System.Func<Product, bool>>>(),
                It.IsAny<Func<IQueryable<Product>, IQueryable<Product>>>()
            )).ReturnsAsync(product);

            var result = await _service.GetByIdAsync(1);

            result.Should().NotBeNull();
            result!.Name.Should().Be("product a");
        }

        [Fact]
        public async Task CreateAsync_AddProduct()
        {
            var dto = new ProductDtoUI
            {
                Name = "product a",
                UnitPrice = 10,
                UnitsInStock = 100,
                CategoryId = 1,
                SupplierId = 1
            };

            var category = new Category { Id = 1, Name = "category a" };
            var supplier = new Supplier { Id = 1, Name = "supplier a" };
            var mappedProduct = new Product { Name = dto.Name };

            _categoryRepoMock.Setup(r => r.GetByIdAsync(dto.CategoryId)).ReturnsAsync(category);
            _supplierRepoMock.Setup(r => r.GetByIdAsync(dto.SupplierId)).ReturnsAsync(supplier);
            _mapperMock.Setup(m => m.Map<Product>(dto)).Returns(mappedProduct);

            var result = await _service.CreateAsync(dto);

            result.Should().BeSameAs(mappedProduct);
            result.Category.Should().Be(category);
            result.Supplier.Should().Be(supplier);

            _productRepoMock.Verify(r => r.AddAsync(mappedProduct), Times.Once);
            _productRepoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_Error()
        {
            var dto = new ProductDtoUI { CategoryId = 99, SupplierId = 1 };

            _categoryRepoMock.Setup(r => r.GetByIdAsync(dto.CategoryId)).ReturnsAsync((Category?)null);

            var act = async () => await _service.CreateAsync(dto);

            await act.Should().ThrowAsync<Exception>().WithMessage("Category not found");
        }

        [Fact]
        public async Task UpdateAsync_UpdateProduct()
        {
            var existing = new Product { Id = 1, Name = "product a", Deleted = false, Category = new Category(), Supplier = new Supplier() };
            var dto = new ProductDtoUI
            {
                Name = "product b",
                CategoryId = 5,
                SupplierId = 6
            };

            var category = new Category { Id = 5, Name = "category" };
            var supplier = new Supplier { Id = 6, Name = "supplier" };

            _productRepoMock.Setup(r => r.FirstOrDefaultAsync(
                It.IsAny<System.Linq.Expressions.Expression<System.Func<Product, bool>>>(),
                It.IsAny<Func<IQueryable<Product>, IQueryable<Product>>>()
            )).ReturnsAsync(existing);

            _categoryRepoMock.Setup(r => r.GetByIdAsync(dto.CategoryId)).ReturnsAsync(category);
            _supplierRepoMock.Setup(r => r.GetByIdAsync(dto.SupplierId)).ReturnsAsync(supplier);
            _mapperMock.Setup(m => m.Map(dto, existing)).Callback(() => existing.Name = dto.Name);

            var result = await _service.UpdateAsync(1, dto);

            result.Should().BeTrue();
            existing.Name.Should().Be("product b");
            existing.Category.Should().Be(category);
            existing.Supplier.Should().Be(supplier);

            _productRepoMock.Verify(r => r.Update(existing), Times.Once);
            _productRepoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ReturnFalse()
        {
            _productRepoMock.Setup(r => r.FirstOrDefaultAsync(
                It.IsAny<System.Linq.Expressions.Expression<System.Func<Product, bool>>>(),
                It.IsAny<Func<IQueryable<Product>, IQueryable<Product>>>()
            )).ReturnsAsync((Product?)null);

            var dto = new ProductDtoUI();
            var result = await _service.UpdateAsync(1, dto);

            result.Should().BeFalse();
        }

        [Fact]
        public async Task DeleteAsync_UpdateDeleted()
        {
            var product = new Product { Id = 1, Deleted = false };
            _productRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(product);

            var result = await _service.DeleteAsync(1);

            result.Should().BeTrue();
            product.Deleted.Should().BeTrue();

            _productRepoMock.Verify(r => r.Update(product), Times.Once);
            _productRepoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ReturnFalse()
        {
            _productRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Product?)null);

            var result = await _service.DeleteAsync(1);

            result.Should().BeFalse();
        }
    }
}
