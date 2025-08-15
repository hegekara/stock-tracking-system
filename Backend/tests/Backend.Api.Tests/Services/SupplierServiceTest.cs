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
    public class SupplierServiceTests
    {
        private readonly Mock<IRepository<Supplier>> _supplierRepoMock;
        private readonly Mock<IRepository<Address>> _addressRepoMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly SupplierService _service;

        public SupplierServiceTests()
        {
            _supplierRepoMock = new Mock<IRepository<Supplier>>();
            _addressRepoMock = new Mock<IRepository<Address>>();
            _mapperMock = new Mock<IMapper>();

            _service = new SupplierService(
                _supplierRepoMock.Object,
                _addressRepoMock.Object,
                _mapperMock.Object
            );
        }

        [Fact]
        public async Task GetAllAsync_ReturnSuppliers()
        {
            var suppliers = new List<Supplier>
            {
                new Supplier { Id = 1, Name = "firma a", Deleted = false, Address = new Address() },
                new Supplier { Id = 2, Name = "firma b", Deleted = null, Address = new Address() },
                new Supplier { Id = 3, Name = "firma c", Deleted = true, Address = new Address() }
            };

            _supplierRepoMock.Setup(r =>
                r.FindAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<Supplier, bool>>>(),
                            It.IsAny<Func<IQueryable<Supplier>, IQueryable<Supplier>>>()))
                .ReturnsAsync(suppliers.Where(s => s.Deleted == false || s.Deleted == null));

            var result = await _service.GetAllAsync();

            result.Should().HaveCount(2);
            result.Should().OnlyContain(s => s.Deleted == false || s.Deleted == null);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnSupplier()
        {
            var supplier = new Supplier { Id = 1, Name = "firma a", Deleted = false, Address = new Address() };
            _supplierRepoMock.Setup(r =>
                r.FirstOrDefaultAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<Supplier, bool>>>(),
                                      It.IsAny<Func<IQueryable<Supplier>, IQueryable<Supplier>>>()))
                .ReturnsAsync(supplier);

            var result = await _service.GetByIdAsync(1);

            result.Should().NotBeNull();
            result!.Name.Should().Be("firma a");
        }

        [Fact]
        public async Task AddAsync_AddSupplier()
        {
            var dto = new SupplierDtoIU
            {
                Name = "firma a",
                ContactName = "ege",
                Phone = "5467389163",
                AddressId = 10
            };

            var mappedSupplier = new Supplier
            {
                Name = dto.Name,
                ContactName = dto.ContactName,
                Phone = dto.Phone
            };

            var address = new Address { Id = 10, CityName = "Ankara", DistrictName = "Çankaya" };

            _mapperMock.Setup(m => m.Map<Supplier>(dto)).Returns(mappedSupplier);
            _addressRepoMock.Setup(r => r.GetByIdAsync(dto.AddressId)).ReturnsAsync(address);

            var result = await _service.AddAsync(dto);

            result.Should().BeSameAs(mappedSupplier);
            result.Address.Should().Be(address);

            _supplierRepoMock.Verify(r => r.AddAsync(mappedSupplier), Times.Once);
            _supplierRepoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task AddAsync_Error()
        {
            var dto = new SupplierDtoIU { AddressId = 99 };
            var mappedSupplier = new Supplier();

            _mapperMock.Setup(m => m.Map<Supplier>(dto)).Returns(mappedSupplier);
            _addressRepoMock.Setup(r => r.GetByIdAsync(dto.AddressId)).ReturnsAsync((Address?)null);

            var act = async () => await _service.AddAsync(dto);

            await act.Should().ThrowAsync<Exception>()
                     .WithMessage("Address not found");
        }

        [Fact]
        public async Task UpdateAsync_UpdateSupplier()
        {
            var existing = new Supplier { Id = 1, Name = "firma a", Deleted = false, Address = new Address() };
            var dto = new SupplierDtoIU
            {
                Name = "firma b",
                ContactName = "ege",
                Phone = "5463782649",
                AddressId = 5
            };

            var address = new Address { Id = 5, CityName = "Ankara", DistrictName = "Çankaya" };

            _supplierRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(existing);
            _mapperMock.Setup(m => m.Map(dto, existing)).Callback(() =>
            {
                existing.Name = dto.Name;
                existing.ContactName = dto.ContactName;
                existing.Phone = dto.Phone;
            });
            _addressRepoMock.Setup(r => r.GetByIdAsync(dto.AddressId)).ReturnsAsync(address);

            var result = await _service.UpdateAsync(1, dto);

            result.Should().NotBeNull();
            result!.Name.Should().Be("firma b");
            result.Address.Should().Be(address);

            _supplierRepoMock.Verify(r => r.Update(existing), Times.Once);
            _supplierRepoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_UpdateDeleted()
        {
            var supplier = new Supplier { Id = 1, Name = "firma a", Deleted = false };
            _supplierRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(supplier);

            var result = await _service.DeleteAsync(1);

            result.Should().BeTrue();
            supplier.Deleted.Should().BeTrue();

            _supplierRepoMock.Verify(r => r.Update(supplier), Times.Once);
            _supplierRepoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }
    }
}
