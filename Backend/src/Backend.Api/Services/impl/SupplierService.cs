using AutoMapper;
using Backend.Api.Dtos;
using Backend.Api.Models;
using Backend.Api.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Backend.Api.Services
{
    public class SupplierService : ISupplierService
    {
        private readonly IRepository<Supplier> _supplierRepository;
        private readonly IRepository<Address> _addressRepository;
        private readonly IMapper _mapper;

        public SupplierService(
            IRepository<Supplier> supplierRepository,
            IRepository<Address> addressRepository,
            IMapper mapper)
        {
            _supplierRepository = supplierRepository;
            _addressRepository = addressRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Supplier>> GetAllAsync()
        {
            var suppliers = await _supplierRepository
                .FindAsync(s => s.Deleted == false || s.Deleted == null, include: q =>
                    q.Include(x => x.Address));

            return suppliers;
        }

        public async Task<Supplier?> GetByIdAsync(int id)
        {
            var supplier = await _supplierRepository
                .FirstOrDefaultAsync(s => s.Id == id && (s.Deleted == false || s.Deleted == null),
                    include: q => q.Include(x => x.Address));

            return supplier == null ? null : supplier;
        }

        public async Task<Supplier> AddAsync(SupplierDtoIU dto)
        {
            var supplier = _mapper.Map<Supplier>(dto);
            supplier.Address = await _addressRepository.GetByIdAsync(dto.AddressId)
                ?? throw new Exception("Address not found");

            await _supplierRepository.AddAsync(supplier);
            await _supplierRepository.SaveChangesAsync();

            return supplier;
        }

        public async Task<Supplier?> UpdateAsync(int id, SupplierDtoIU dto)
        {
            var existing = await _supplierRepository.GetByIdAsync(id);
            if (existing == null || existing.Deleted == true)
                return null;

            _mapper.Map(dto, existing);
            existing.Address = await _addressRepository.GetByIdAsync(dto.AddressId)
                ?? throw new Exception("Address not found");

            _supplierRepository.Update(existing);
            await _supplierRepository.SaveChangesAsync();

            return existing;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var supplier = await _supplierRepository.GetByIdAsync(id);
            if (supplier == null || supplier.Deleted == true)
                return false;

            supplier.Deleted = true;
            _supplierRepository.Update(supplier);
            await _supplierRepository.SaveChangesAsync();
            return true;
        }
    }
}
