using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Backend.Api.Dtos;
using Backend.Api.Models;
using Backend.Api.Repositories;
using Backend.Api.Services;
using FluentAssertions;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Xunit;

public class StockTransactionServiceTests
{
    private readonly Mock<IRepository<StockTransaction>> _transactionRepoMock;
    private readonly Mock<IRepository<Product>> _productRepoMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly StockTransactionService _service;

    public StockTransactionServiceTests()
    {
        _transactionRepoMock = new Mock<IRepository<StockTransaction>>();
        _productRepoMock = new Mock<IRepository<Product>>();
        _mapperMock = new Mock<IMapper>();

        _service = new StockTransactionService(
            _transactionRepoMock.Object,
            _productRepoMock.Object,
            _mapperMock.Object
        );
    }

    [Fact]
    public async Task GetAllAsync_ReturnTransactions()
    {
        var transactions = new List<StockTransaction> { new StockTransaction { Id = 1 } };
        _transactionRepoMock.Setup(r => r.FindAsync(
            It.IsAny<System.Linq.Expressions.Expression<Func<StockTransaction, bool>>>(),
            It.IsAny<Func<IQueryable<StockTransaction>, IQueryable<StockTransaction>>>()))
            .ReturnsAsync(transactions);

        var result = await _service.GetAllAsync();

        result.Should().HaveCount(1);
        _transactionRepoMock.Verify(r => r.FindAsync(
            It.IsAny<System.Linq.Expressions.Expression<Func<StockTransaction, bool>>>(),
            It.IsAny<Func<IQueryable<StockTransaction>, IQueryable<StockTransaction>>>()),
            Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnTransaction()
    {
        var transaction = new StockTransaction { Id = 5 };
        _transactionRepoMock.Setup(r => r.FirstOrDefaultAsync(
            It.IsAny<System.Linq.Expressions.Expression<Func<StockTransaction, bool>>>(),
            It.IsAny<Func<IQueryable<StockTransaction>, IQueryable<StockTransaction>>>()))
            .ReturnsAsync(transaction);

        var result = await _service.GetByIdAsync(5);

        result.Should().NotBeNull();
        result.Id.Should().Be(5);
    }

    [Fact]
    public async Task AddStockAsync_Error_NotFound()
    {
        _productRepoMock.Setup(r => r.FirstOrDefaultAsync(
            It.IsAny<System.Linq.Expressions.Expression<Func<Product, bool>>>(),
            It.IsAny<Func<IQueryable<Product>, IQueryable<Product>>>()))
            .ReturnsAsync((Product?)null);

        var dto = new StockTransactionDtoIU { ProductId = 99, Quantity = 5 };

        await Assert.ThrowsAsync<Exception>(() => _service.AddStockAsync(dto));
    }

    [Fact]
    public async Task AddStockAsync_IncreaseStock()
    {
        var product = new Product { Id = 1, UnitsInStock = 10 };
        var dto = new StockTransactionDtoIU { ProductId = 1, Quantity = 5 };
        var transaction = new StockTransaction { Id = 1 };

        _productRepoMock.Setup(r => r.FirstOrDefaultAsync(
            It.IsAny<System.Linq.Expressions.Expression<Func<Product, bool>>>(),
            It.IsAny<Func<IQueryable<Product>, IQueryable<Product>>>()))
            .ReturnsAsync(product);

        _mapperMock.Setup(m => m.Map<StockTransaction>(dto)).Returns(transaction);

        var result = await _service.AddStockAsync(dto);

        product.UnitsInStock.Should().Be(15);
        _transactionRepoMock.Verify(r => r.AddAsync(transaction), Times.Once);
        _productRepoMock.Verify(r => r.Update(product), Times.Once);
        _transactionRepoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task RemoveStockAsync_Error_NotFound()
    {
        _productRepoMock.Setup(r => r.FirstOrDefaultAsync(
            It.IsAny<System.Linq.Expressions.Expression<Func<Product, bool>>>(),
            It.IsAny<Func<IQueryable<Product>, IQueryable<Product>>>()))
            .ReturnsAsync((Product?)null);

        var dto = new StockTransactionDtoIU { ProductId = 99, Quantity = 5 };

        await Assert.ThrowsAsync<Exception>(() => _service.RemoveStockAsync(dto));
    }

    [Fact]
    public async Task RemoveStockAsync_Error_NotEnoughStock()
    {
        var product = new Product { Id = 1, UnitsInStock = 3 };
        var dto = new StockTransactionDtoIU { ProductId = 1, Quantity = 5 };

        _productRepoMock.Setup(r => r.FirstOrDefaultAsync(
            It.IsAny<System.Linq.Expressions.Expression<Func<Product, bool>>>(),
            It.IsAny<Func<IQueryable<Product>, IQueryable<Product>>>()))
            .ReturnsAsync(product);

        await Assert.ThrowsAsync<Exception>(() => _service.RemoveStockAsync(dto));
    }

    [Fact]
    public async Task RemoveStockAsync_DecreaseStock()
    {
        var product = new Product { Id = 1, UnitsInStock = 10 };
        var dto = new StockTransactionDtoIU { ProductId = 1, Quantity = 4 };
        var transaction = new StockTransaction { Id = 1 };

        _productRepoMock.Setup(r => r.FirstOrDefaultAsync(
            It.IsAny<System.Linq.Expressions.Expression<Func<Product, bool>>>(),
            It.IsAny<Func<IQueryable<Product>, IQueryable<Product>>>()))
            .ReturnsAsync(product);

        _mapperMock.Setup(m => m.Map<StockTransaction>(dto)).Returns(transaction);

        var result = await _service.RemoveStockAsync(dto);

        product.UnitsInStock.Should().Be(6);
        _transactionRepoMock.Verify(r => r.AddAsync(transaction), Times.Once);
        _productRepoMock.Verify(r => r.Update(product), Times.Once);
        _transactionRepoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
    }
}
