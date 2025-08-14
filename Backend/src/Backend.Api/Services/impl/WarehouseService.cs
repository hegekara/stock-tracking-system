using AutoMapper;
using Backend.Api.Dtos;
using Backend.Api.Models;
using Backend.Api.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Backend.Api.Services
{
    public class WarehouseService : IWarehouseService
    {
        private readonly IRepository<Warehouse> _warehouseRepository;
        private readonly IRepository<Address> _addressRepository;
        private readonly IMapper _mapper;

        public WarehouseService(IRepository<Warehouse> warehouseRepository,
                                 IRepository<Address> addressRepository,
                                 IMapper mapper)
        {
            _warehouseRepository = warehouseRepository;
            _addressRepository = addressRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Warehouse>> GetAllAsync()
        {
            return await _warehouseRepository.GetAllAsync(include: q => q.Include(x => x.Address)
                .Where(w => w.Deleted == false || w.Deleted == null));
        }

        public async Task<Warehouse?> GetByIdAsync(int id)
        {
            return await _warehouseRepository.FirstOrDefaultAsync(
                s => s.Id == id && (s.Deleted == false || s.Deleted == null),
                include: q => q.Include(x => x.Address)
            );
        }

        public async Task<Warehouse> CreateAsync(WarehouseDtoIU dto)
        {
            var warehouse = _mapper.Map<Warehouse>(dto);

            warehouse.Address = await _addressRepository.GetByIdAsync(dto.AddressId)
                ?? throw new Exception("Address not found");

            await _warehouseRepository.AddAsync(warehouse);
            await _warehouseRepository.SaveChangesAsync();

            return warehouse;
        }

        public async Task<Warehouse?> UpdateAsync(int id, WarehouseDtoIU dto)
        {
            var existing = await _warehouseRepository.FirstOrDefaultAsync(
                w => w.Id == id && (w.Deleted == false || w.Deleted == null),
                include: q => q.Include(x => x.Address)
            );

            if (existing == null)
                return null;

            _mapper.Map(dto, existing);

            existing.Address = await _addressRepository.GetByIdAsync(dto.AddressId)
                ?? throw new Exception("Address not found");

            _warehouseRepository.Update(existing);
            await _warehouseRepository.SaveChangesAsync();

            return existing;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var warehouse = await _warehouseRepository.GetByIdAsync(id);
            if (warehouse == null)
                return false;

            warehouse.Deleted = true;
            _warehouseRepository.Update(warehouse);
            await _warehouseRepository.SaveChangesAsync();

            return true;
        }
    }
}
