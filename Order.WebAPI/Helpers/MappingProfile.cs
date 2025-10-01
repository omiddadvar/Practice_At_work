using AutoMapper;
using Order.WebAPI.Models.DTOs;
using Order.WebAPI.Models.Entities;
using ORDER = Order.WebAPI.Models.Entities.Order;

namespace Order.WebAPI.Helpers;


public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Customer mappings
        CreateMap<Customer, CustomerDto>();
        CreateMap<CreateCustomerDto, Customer>();
        CreateMap<UpdateCustomerDto, Customer>();

        // Order mappings
        CreateMap<ORDER, OrderDto>()
            .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => $"{src.Customer.FirstName} {src.Customer.LastName}"))
            .ForMember(dest => dest.CustomerEmail, opt => opt.MapFrom(src => src.Customer.Email));
        CreateMap<CreateOrderDto, ORDER>();
        CreateMap<UpdateOrderDto, ORDER>();

        // OrderItem mappings
        CreateMap<OrderItem, OrderItemDto>()
            .ForMember(dest => dest.TotalPrice, opt => opt.MapFrom(src => src.UnitPrice * src.Quantity));
        CreateMap<CreateOrderItemDto, OrderItem>();
        CreateMap<UpdateOrderItemDto, OrderItem>();
    }
}