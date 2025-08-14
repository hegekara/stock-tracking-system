using AutoMapper;
using Backend.Api.Models;
using Backend.Api.Repositories;
using Backend.Api.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Backend.Api.Services
{
    public class OrderService : IOrderService
    {
        private readonly IRepository<Order> _orderRepository;
        private readonly IRepository<Customer> _customerRepository;
        private readonly IMapper _mapper;

        public OrderService(IRepository<Order> orderRepository, IRepository<Customer> customerRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _customerRepository = customerRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            return await _orderRepository.FindAsync(
                o => o.Deleted == false || o.Deleted == null,
                include: q => q
                    .Include(x => x.Customer)
                        .ThenInclude(c => c.Address)
            );
        }

        public async Task<Order?> GetByIdAsync(int id)
        {
            var order = await _orderRepository.FirstOrDefaultAsync(
                c => c.Id == id && (c.Deleted == false || c.Deleted == null),
                include: q => q
                    .Include(x => x.Customer)
                        .ThenInclude(c => c.Address)
            );

            if (order == null || order.Deleted == true)
                return null;

            return order;
        }


        public async Task<Order> AddAsync(OrderDtoIU orderDto)
        {
            var customer = await _customerRepository.GetByIdAsync(orderDto.CustomerId);
            if (customer == null || customer.Deleted == true)
                throw new Exception("Customer not found");

            var order = _mapper.Map<Order>(orderDto);
            order.Customer = customer;
            order.OrderDate = DateTime.Now;

            await _orderRepository.AddAsync(order);
            await _orderRepository.SaveChangesAsync();
            return order;
        }

        public async Task<Order?> UpdateAsync(int id, OrderDtoIU orderDto)
        {
            var existingOrder = await _orderRepository.GetByIdAsync(id);
            if (existingOrder == null || existingOrder.Deleted == true)
                return null;

            var customer = await _customerRepository.GetByIdAsync(orderDto.CustomerId);
            if (customer == null || customer.Deleted == true)
                throw new Exception("Customer not found");

            _mapper.Map(orderDto, existingOrder);
            existingOrder.Customer = customer;

            _orderRepository.Update(existingOrder);
            await _orderRepository.SaveChangesAsync();
            return existingOrder;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null || order.Deleted == true)
                return false;

            order.Deleted = true;
            _orderRepository.Update(order);
            await _orderRepository.SaveChangesAsync();
            return true;
        }
    }
}
