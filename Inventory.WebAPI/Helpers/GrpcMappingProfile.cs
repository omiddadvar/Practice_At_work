using AutoMapper;
using Inventory.WebAPI.Models.DTOs;
using Inventory.WebAPI.Protos;

namespace Inventory.WebAPI.Helpers;

public class GrpcMappingProfile : Profile
{
    public GrpcMappingProfile()
    {
        CreateMap<ProductDto, ProductResponse>()
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => (double)src.Price));

        CreateMap<CategoryDto, CategoryResponse>();
    }
}