using Microsoft.EntityFrameworkCore;
using Order.WebAPI.Abstractions.Repositories;
using Order.WebAPI.Data;
using ORDER = Order.WebAPI.Models.Entities.Order;

namespace Order.WebAPI.Repositories;

public class OrderRepository : RepositoryBase<ORDER>, IOrderRepository
{
    public OrderRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<ORDER?> GetByOrderNumberAsync(string orderNumber)
    {
        return await Context.Orders
            .Include(o => o.Customer)
            .Include(o => o.OrderItems)
            .FirstOrDefaultAsync(o => o.OrderNumber == orderNumber);
    }

    public async Task<bool> OrderExistsAsync(string orderNumber)
    {
        return await Context.Orders
            .AnyAsync(o => o.OrderNumber == orderNumber);
    }

    public async Task<IEnumerable<ORDER>> GetOrdersWithDetailsAsync()
    {
        return await Context.Orders
            .Include(o => o.Customer)
            .Include(o => o.OrderItems)
            .OrderByDescending(o => o.OrderDate)
            .ToListAsync();
    }

    public async Task<ORDER?> GetOrderWithDetailsAsync(int id)
    {
        return await Context.Orders
            .Include(o => o.Customer)
            .Include(o => o.OrderItems)
            .FirstOrDefaultAsync(o => o.Id == id);
    }

    public async Task<IEnumerable<ORDER>> GetOrdersByCustomerAsync(int customerId)
    {
        return await Context.Orders
            .Include(o => o.Customer)
            .Include(o => o.OrderItems)
            .Where(o => o.CustomerId == customerId)
            .OrderByDescending(o => o.OrderDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<ORDER>> GetOrdersByStatusAsync(string status)
    {
        return await Context.Orders
            .Include(o => o.Customer)
            .Include(o => o.OrderItems)
            .Where(o => o.Status == status)
            .OrderByDescending(o => o.OrderDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<ORDER>> GetOrdersByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        return await Context.Orders
            .Include(o => o.Customer)
            .Include(o => o.OrderItems)
            .Where(o => o.OrderDate >= startDate && o.OrderDate <= endDate)
            .OrderByDescending(o => o.OrderDate)
            .ToListAsync();
    }
}