using Microsoft.EntityFrameworkCore;
using Order.WebAPI.Abstractions.Repositories;
using Order.WebAPI.Data;
using Order.WebAPI.Models.Entities;

namespace Order.WebAPI.Repositories;

public class OrderItemRepository : RepositoryBase<OrderItem>, IOrderItemRepository
{
    public OrderItemRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<OrderItem>> GetOrderItemsByOrderAsync(int orderId)
    {
        return await Context.OrderItems
            .Include(oi => oi.Order)
            .Where(oi => oi.OrderId == orderId)
            .ToListAsync();
    }

    public async Task<IEnumerable<OrderItem>> GetOrderItemsByProductAsync(string productSku)
    {
        return await Context.OrderItems
            .Include(oi => oi.Order)
            .Where(oi => oi.ProductSku == productSku)
            .ToListAsync();
    }
}