using AutoMapper;
using Order.WebAPI.Models.DTOs;
using Order.WebAPI.Protos;

namespace Order.WebAPI.Helpers;

public class GrpcMappingProfile : Profile
{
    public GrpcMappingProfile()
    {
        CreateMap<ProductResponse, ProductDto>()
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => (decimal)src.Price));

        CreateMap<CategoryResponse, CategoryDto>();
    }
}