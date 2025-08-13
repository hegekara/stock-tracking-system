using AutoMapper;
using Backend.Api.DTOs;
using Backend.Api.Models;

namespace Backend.Api.Mapping
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<OrderDtoIU, Order>()
                .ForMember(dest => dest.Customer, opt => opt.Ignore());
        }
    }
}
