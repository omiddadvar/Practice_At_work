namespace Order.WebAPI.Abstractions.Repositories;
using ORDER = Models.Entities.Order;

public interface IOrderRepository : IRepositoryBase<ORDER>
{
    Task<ORDER?> GetByOrderNumberAsync(string orderNumber);
    Task<bool> OrderExistsAsync(string orderNumber);
    Task<IEnumerable<ORDER>> GetOrdersWithDetailsAsync();
    Task<ORDER?> GetOrderWithDetailsAsync(int id);
    Task<IEnumerable<ORDER>> GetOrdersByCustomerAsync(int customerId);
    Task<IEnumerable<ORDER>> GetOrdersByStatusAsync(string status);
    Task<IEnumerable<ORDER>> GetOrdersByDateRangeAsync(DateTime startDate, DateTime endDate);
}