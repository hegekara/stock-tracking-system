using AutoMapper;
using Backend.Api.Models;
using Backend.Api.Dtos;

namespace Backend.Api.Mapping
{
    public class StockTransactionProfile : Profile
    {
        public StockTransactionProfile()
        {
            CreateMap<StockTransactionDtoIU, StockTransaction>()
                .ForMember(dest => dest.Product, opt => opt.Ignore());

            CreateMap<StockTransaction, StockTransactionDto>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name));
        }
    }
}
