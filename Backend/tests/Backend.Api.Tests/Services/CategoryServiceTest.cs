using Xunit;
using Moq;
using FluentAssertions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Api.Models;
using Backend.Api.Repositories;
using Backend.Api.Services;

namespace Backend.Api.Tests.Services
{
    public class CategoryServiceTests
    {
        private readonly Mock<IRepository<Category>> _repoMock;
        private readonly CategoryService _service;

        public CategoryServiceTests()
        {
            _repoMock = new Mock<IRepository<Category>>();
            _service = new CategoryService(_repoMock.Object);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnCategory()
        {
            var category = new Category { Id = 1, Name = "category a", Deleted = false };
            _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(category);

            var result = await _service.GetByIdAsync(1);

            result.Should().NotBeNull();
            result!.Name.Should().Be("category a");
        }

        [Fact]
        public async Task GetByIdAsync_ReturnNull()
        {
            var category = new Category { Id = 1, Name = "category a", Deleted = true };
            _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(category);

            var result = await _service.GetByIdAsync(1);

            result.Should().BeNull();
        }

        [Fact]
        public async Task AddAsync_AddCategory()
        {
            var newCategory = new Category { Name = "category a", Deleted = false };

            var result = await _service.AddAsync(newCategory);

            result.Should().BeSameAs(newCategory);
            _repoMock.Verify(r => r.AddAsync(newCategory), Times.Once);
            _repoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_UpdateCategory()
        {
            var existing = new Category { Id = 1, Name = "category a", Description = "desc a", Deleted = false };
            var updated = new Category { Name = "category b", Description = "desc b" };

            _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(existing);

            var result = await _service.UpdateAsync(1, updated);

            result.Should().NotBeNull();
            result!.Name.Should().Be("category b");
            result.Description.Should().Be("desc b");

            _repoMock.Verify(r => r.Update(existing), Times.Once);
            _repoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_UpdateDeleted()
        {
            var category = new Category { Id = 1, Name = "category a", Deleted = false };
            _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(category);

            var result = await _service.DeleteAsync(1);

            result.Should().BeTrue();
            category.Deleted.Should().BeTrue();

            _repoMock.Verify(r => r.Update(category), Times.Once);
            _repoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }
    }
}
