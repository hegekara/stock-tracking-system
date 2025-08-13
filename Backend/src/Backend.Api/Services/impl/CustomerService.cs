using AutoMapper;
using Backend.Api.Models;
using Backend.Api.Repositories;

namespace Backend.Api.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IRepository<Customer> _repository;
        private readonly IRepository<Address> _addressRepository;
        private readonly IMapper _mapper;

        public CustomerService(
            IRepository<Customer> repository,
            IRepository<Address> addressRepository,
            IMapper mapper)
        {
            _repository = repository;
            _addressRepository = addressRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Customer>> GetAllAsync()
        {
            return await _repository.FindAsync(c => c.Deleted == false || c.Deleted == null);
        }

        public async Task<Customer?> GetByIdAsync(int id)
        {
            var customer = await _repository.GetByIdAsync(id);
            if (customer == null || customer.Deleted == true)
                return null;

            return customer;
        }

        public async Task<Customer> AddAsync(CustomerDtoIU customerDto)
        {
            // DTO -> Customer map
            var customer = _mapper.Map<Customer>(customerDto);

            // Address'i ID üzerinden çek
            var address = await _addressRepository.GetByIdAsync(customerDto.AddressId);
            if (address == null)
                throw new Exception($"Address with ID {customerDto.AddressId} not found");

            customer.Address = address;

            await _repository.AddAsync(customer);
            await _repository.SaveChangesAsync();
            return customer;
        }

        public async Task<Customer?> UpdateAsync(int id, CustomerDtoIU customerDto)
        {
            var existingCustomer = await _repository.GetByIdAsync(id);
            if (existingCustomer == null || existingCustomer.Deleted == true)
                return null;

            _mapper.Map(customerDto, existingCustomer);

            var address = await _addressRepository.GetByIdAsync(customerDto.AddressId);
            if (address == null)
                throw new Exception($"Address with ID {customerDto.AddressId} not found");

            existingCustomer.Address = address;

            _repository.Update(existingCustomer);
            await _repository.SaveChangesAsync();
            return existingCustomer;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var customer = await _repository.GetByIdAsync(id);
            if (customer == null || customer.Deleted == true)
                return false;

            customer.Deleted = true;
            _repository.Update(customer);
            await _repository.SaveChangesAsync();
            return true;
        }
    }
}
