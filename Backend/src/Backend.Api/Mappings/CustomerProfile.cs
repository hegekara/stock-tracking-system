using AutoMapper;
using Backend.Api.Models;

namespace Backend.Api.Mappings
{
    public class CustomerProfile : Profile
    {
        public CustomerProfile()
        {
            CreateMap<CustomerDtoIU, Customer>()
                .ForMember(dest => dest.Address, opt => opt.Ignore());
        }
    }
}
