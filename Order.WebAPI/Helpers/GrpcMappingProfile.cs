using AutoMapper;
using Order.WebAPI.Models.DTOs;
using Order.WebAPI.Protos;

namespace Order.WebAPI.Helpers;

public class GrpcMappingProfile : Profile
{
    public GrpcMappingProfile()
    {
        CreateMap<ProductResponse, ProductDto>()
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => (decimal)src.Price))
            .ForMember(dest => dest.SKU, opt => opt.MapFrom(src => src.Sku)); // Map Sku to SKU

        CreateMap<CategoryResponse, CategoryDto>();
    }
}