using Order.WebAPI.Models.DTOs;

namespace Order.WebAPI.Abstractions.Services;

public interface IOrderService
{
    Task<IEnumerable<OrderDto>> GetOrdersAsync();
    Task<OrderDto?> GetOrderByIdAsync(int id);
    Task<OrderDto?> GetOrderByNumberAsync(string orderNumber);
    Task<OrderDto> CreateOrderAsync(CreateOrderDto createOrderDto);
    Task<OrderDto?> UpdateOrderAsync(int id, UpdateOrderDto updateOrderDto);
    Task<OrderDto?> UpdateOrderStatusAsync(int id, UpdateOrderStatusDto updateOrderStatusDto);
    Task<bool> DeleteOrderAsync(int id);
    Task<IEnumerable<OrderDto>> GetOrdersByStatusAsync(string status);
    Task<IEnumerable<OrderDto>> GetOrdersByDateRangeAsync(DateTime startDate, DateTime endDate);
}