using AutoMapper;
using Order.WebAPI.Abstractions.Repositories;
using Order.WebAPI.Abstractions.Services;
using Order.WebAPI.Models.DTOs;
using Order.WebAPI.Models.Entities;

namespace Order.WebAPI.Services;

public class OrderItemService : IOrderItemService
{
    private readonly IOrderItemRepository _orderItemRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly IMapper _mapper;

    public OrderItemService(IOrderItemRepository orderItemRepository, IOrderRepository orderRepository, IMapper mapper)
    {
        _orderItemRepository = orderItemRepository;
        _orderRepository = orderRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<OrderItemDto>> GetOrderItemsAsync()
    {
        var orderItems = await _orderItemRepository.FindAllAsync();
        return _mapper.Map<IEnumerable<OrderItemDto>>(orderItems);
    }

    public async Task<OrderItemDto?> GetOrderItemByIdAsync(int id)
    {
        var orderItem = await _orderItemRepository.FindByIdAsync(id);
        return orderItem == null ? null : _mapper.Map<OrderItemDto>(orderItem);
    }

    public async Task<OrderItemDto> CreateOrderItemAsync(CreateOrderItemDto createOrderItemDto)
    {
        var orderItem = _mapper.Map<OrderItem>(createOrderItemDto);
        var createdOrderItem = await _orderItemRepository.CreateAsync(orderItem);
        return _mapper.Map<OrderItemDto>(createdOrderItem);
    }

    public async Task<OrderItemDto?> UpdateOrderItemAsync(int id, UpdateOrderItemDto updateOrderItemDto)
    {
        var orderItem = await _orderItemRepository.FindByIdAsync(id);
        if (orderItem == null) return null;

        _mapper.Map(updateOrderItemDto, orderItem);
        var updatedOrderItem = await _orderItemRepository.UpdateAsync(orderItem);
        return _mapper.Map<OrderItemDto>(updatedOrderItem);
    }

    public async Task<bool> DeleteOrderItemAsync(int id)
    {
        var orderItem = await _orderItemRepository.FindByIdAsync(id);
        if (orderItem == null) return false;

        await _orderItemRepository.DeleteAsync(orderItem);
        return true;
    }

    public async Task<IEnumerable<OrderItemDto>> GetOrderItemsByOrderAsync(int orderId)
    {
        var orderItems = await _orderItemRepository.GetOrderItemsByOrderAsync(orderId);
        return _mapper.Map<IEnumerable<OrderItemDto>>(orderItems);
    }

    public async Task<IEnumerable<OrderItemDto>> GetOrderItemsByProductAsync(string productSku)
    {
        var orderItems = await _orderItemRepository.GetOrderItemsByProductAsync(productSku);
        return _mapper.Map<IEnumerable<OrderItemDto>>(orderItems);
    }
}