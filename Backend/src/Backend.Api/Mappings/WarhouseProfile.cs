using AutoMapper;
using Backend.Api.Dtos;
using Backend.Api.Models;

namespace Backend.Api.MappingProfiles
{
    public class WarehouseProfile : Profile
    {
        public WarehouseProfile()
        {
            CreateMap<WarehouseDtoIU, Warehouse>()
                .ForMember(dest => dest.Address, opt => opt.Ignore());
        }
    }
}
