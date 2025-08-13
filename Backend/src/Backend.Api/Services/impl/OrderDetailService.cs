using Backend.Api.DTOs;
using Backend.Api.Models;
using Backend.Api.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Backend.Api.Services
{
    public class OrderDetailService : IOrderDetailService
    {
        private readonly IRepository<OrderDetail> _orderDetailRepository;
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<Order> _orderRepository;

        public OrderDetailService(
            IRepository<OrderDetail> orderDetailRepository,
            IRepository<Product> productRepository,
            IRepository<Order> orderRepository)
        {
            _orderDetailRepository = orderDetailRepository;
            _productRepository = productRepository;
            _orderRepository = orderRepository;
        }

        public async Task<IEnumerable<OrderDetailDto>> GetByOrderIdAsync(int orderId)
        {
            var details = await _orderDetailRepository.FindAsync(
                x => x.OrderId == orderId && (x.Deleted == false || x.Deleted == null),
                include: q => q.Include(x => x.Product));

            return details.Select(d => new OrderDetailDto
            {
                Id = d.Id,
                OrderId = d.OrderId,
                ProductId = d.ProductId,
                ProductName = d.Product.Name,
                Quantity = d.Quantity,
                UnitPrice = d.UnitPrice
            });
        }

        public async Task<OrderDetailDto?> GetByIdAsync(int id)
        {
            var detail = await _orderDetailRepository.FirstOrDefaultAsync(
                x => x.Id == id && (x.Deleted == false || x.Deleted == null),
                include: q => q.Include(x => x.Product));

            if (detail == null) return null;

            return new OrderDetailDto
            {
                Id = detail.Id,
                OrderId = detail.OrderId,
                ProductId = detail.ProductId,
                ProductName = detail.Product.Name,
                Quantity = detail.Quantity,
                UnitPrice = detail.UnitPrice
            };
        }

        public async Task<OrderDetailDto> CreateAsync(int orderId, OrderDetailCreateDto dto)
        {
            var orderExists = await _orderRepository.GetByIdAsync(orderId);
            if (orderExists == null)
                throw new Exception("Order not found");

            var product = await _productRepository.GetByIdAsync(dto.ProductId);
            if (product == null)
                throw new Exception("Product not found");

            var detail = new OrderDetail
            {
                OrderId = orderId,
                ProductId = dto.ProductId,
                Quantity = dto.Quantity,
                UnitPrice = dto.UnitPrice
            };

            await _orderDetailRepository.AddAsync(detail);
            await _orderDetailRepository.SaveChangesAsync();

            return new OrderDetailDto
            {
                Id = detail.Id,
                OrderId = detail.OrderId,
                ProductId = detail.ProductId,
                ProductName = product.Name,
                Quantity = detail.Quantity,
                UnitPrice = detail.UnitPrice
            };
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var detail = await _orderDetailRepository.GetByIdAsync(id);
            if (detail == null) return false;

            detail.Deleted = true;
            _orderDetailRepository.Update(detail);
            await _orderDetailRepository.SaveChangesAsync();
            return true;
        }
    }
}
