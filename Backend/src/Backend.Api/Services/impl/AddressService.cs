using Backend.Api.Models;
using Backend.Api.Repositories;

namespace Backend.Api.Services
{
    public class AddressService : IAddressService
    {
        private readonly IRepository<Address> _addressRepository;

        public AddressService(IRepository<Address> addressRepository)
        {
            _addressRepository = addressRepository;
        }

        public async Task<IEnumerable<Address>> GetAllAsync()
        {
            return await _addressRepository.FindAsync(a => a.Deleted == false || a.Deleted == null);
        }

        public async Task<Address?> GetByIdAsync(int id)
        {
            var address = await _addressRepository.GetByIdAsync(id);
            if (address == null || address.Deleted == true)
                return null;

            return address;
        }

        public async Task<Address> CreateAsync(Address address)
        {
            address.Deleted = false;
            await _addressRepository.AddAsync(address);
            await _addressRepository.SaveChangesAsync();
            return address;
        }

        public async Task<Address?> UpdateAsync(int id, Address address)
        {
            var existing = await _addressRepository.GetByIdAsync(id);
            if (existing == null || existing.Deleted == true)
                return null;

            existing.Street = address.Street;
            existing.CityId = address.CityId;
            existing.DistrictId = address.DistrictId;

            _addressRepository.Update(existing);
            await _addressRepository.SaveChangesAsync();

            return existing;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _addressRepository.GetByIdAsync(id);
            if (existing == null || existing.Deleted == true)
                return false;

            existing.Deleted = true;
            _addressRepository.Update(existing);
            await _addressRepository.SaveChangesAsync();

            return true;
        }
    }
}
