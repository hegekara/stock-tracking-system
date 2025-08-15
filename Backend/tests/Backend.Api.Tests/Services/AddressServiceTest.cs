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
    public class AddressServiceTests
    {
        private readonly Mock<IRepository<Address>> _repoMock;
        private readonly AddressService _service;

        public AddressServiceTests()
        {
            _repoMock = new Mock<IRepository<Address>>();
            _service = new AddressService(_repoMock.Object);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnAddress()
        {
            var address = new Address { Id = 1, CityName = "Ankara", Deleted = false };
            _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(address);

            var result = await _service.GetByIdAsync(1);

            result.Should().NotBeNull();
            result!.CityName.Should().Be("Ankara");
        }

        [Fact]
        public async Task GetByIdAsync_ReturnNull()
        {
            var address = new Address { Id = 1, CityName = "İstanbul", Deleted = true };
            _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(address);

            var result = await _service.GetByIdAsync(1);

            result.Should().BeNull();
        }

        [Fact]
        public async Task DeleteAsync_ReturnTrue()
        {
            var address = new Address { Id = 1, CityName = "Bursa", Deleted = false };
            _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(address);

            var result = await _service.DeleteAsync(1);

            result.Should().BeTrue();
            address.Deleted.Should().BeTrue();

            _repoMock.Verify(r => r.Update(address), Times.Once);
            _repoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }
    }
}
