using AutoMapper;
using Backend.Api.Models;
using Backend.Api.Dtos;

namespace Backend.Api.Mapping
{
    public class SupplierProfile : Profile
    {
        public SupplierProfile()
        {
            CreateMap<SupplierDtoIU, Supplier>()
                .ForMember(dest => dest.Address, opt => opt.Ignore());

        }
    }
}
