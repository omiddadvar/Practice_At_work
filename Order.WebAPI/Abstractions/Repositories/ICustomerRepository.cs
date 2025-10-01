using Order.WebAPI.Models.Entities;

namespace Order.WebAPI.Abstractions.Repositories;

public interface ICustomerRepository : IRepositoryBase<Customer>
{
    Task<Customer?> GetByEmailAsync(string email);
    Task<bool> CustomerExistsAsync(string email);
    Task<IEnumerable<Customer>> GetCustomersWithOrdersAsync();
    Task<Customer?> GetCustomerWithOrdersAsync(int id);
}