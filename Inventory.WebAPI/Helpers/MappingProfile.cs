using AutoMapper;
using Inventory.WebAPI.Models.DTOs;
using Inventory.WebAPI.Models.Entities;

namespace Inventory.WebAPI.Helpers;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Category mappings
        CreateMap<Category, CategoryDto>();
        CreateMap<CreateCategoryDto, Category>();
        CreateMap<UpdateCategoryDto, Category>();

        // Product mappings
        CreateMap<Product, ProductDto>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name));
        CreateMap<CreateProductDto, Product>();
        CreateMap<UpdateProductDto, Product>();

        // Warehouse mappings
        CreateMap<WareHouse, WareHouseDto>();
        CreateMap<CreateWareHouseDto, WareHouse>();
        CreateMap<UpdateWareHouseDto, WareHouse>();

        // Stack mappings
        CreateMap<Stack, StackDto>()
            .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name))
            .ForMember(dest => dest.WareHouseName, opt => opt.MapFrom(src => src.WareHouse.Name));
        CreateMap<CreateStackDto, Stack>();
        CreateMap<UpdateStackDto, Stack>();
    }
}
