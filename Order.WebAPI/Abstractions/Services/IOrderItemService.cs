using Order.WebAPI.Models.DTOs;

namespace Order.WebAPI.Abstractions.Services;

public interface IOrderItemService
{
    Task<IEnumerable<OrderItemDto>> GetOrderItemsAsync();
    Task<OrderItemDto?> GetOrderItemByIdAsync(int id);
    Task<OrderItemDto> CreateOrderItemAsync(CreateOrderItemDto createOrderItemDto);
    Task<OrderItemDto?> UpdateOrderItemAsync(int id, UpdateOrderItemDto updateOrderItemDto);
    Task<bool> DeleteOrderItemAsync(int id);
    Task<IEnumerable<OrderItemDto>> GetOrderItemsByOrderAsync(int orderId);
    Task<IEnumerable<OrderItemDto>> GetOrderItemsByProductAsync(string productSku);
}