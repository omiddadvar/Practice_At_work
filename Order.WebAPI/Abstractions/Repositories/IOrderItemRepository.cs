using Order.WebAPI.Models.Entities;

namespace Order.WebAPI.Abstractions.Repositories;

public interface IOrderItemRepository : IRepositoryBase<OrderItem>
{
    Task<IEnumerable<OrderItem>> GetOrderItemsByOrderAsync(int orderId);
    Task<IEnumerable<OrderItem>> GetOrderItemsByProductAsync(string productSku);
    Task<IEnumerable<OrderItem>> GetOrderItemsByProductIdAsync(int productId);
}