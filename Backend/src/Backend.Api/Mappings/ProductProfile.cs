using AutoMapper;
using Backend.Api.Models;
using Backend.Api.Dtos;

namespace Backend.Api.Mapping
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<ProductDtoUI, Product>();
        }
    }
}
