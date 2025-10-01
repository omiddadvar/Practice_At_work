using Microsoft.EntityFrameworkCore;
using Order.WebAPI.Abstractions.Repositories;
using Order.WebAPI.Data;
using Order.WebAPI.Models.Entities;

namespace Order.WebAPI.Repositories;

public class CustomerRepository : RepositoryBase<Customer>, ICustomerRepository
{
    public CustomerRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Customer?> GetByEmailAsync(string email)
    {
        return await Context.Customers
            .FirstOrDefaultAsync(c => c.Email.ToLower() == email.ToLower());
    }

    public async Task<bool> CustomerExistsAsync(string email)
    {
        return await Context.Customers
            .AnyAsync(c => c.Email.ToLower() == email.ToLower());
    }

    public async Task<IEnumerable<Customer>> GetCustomersWithOrdersAsync()
    {
        return await Context.Customers
            .Include(c => c.Orders)
            .ToListAsync();
    }

    public async Task<Customer?> GetCustomerWithOrdersAsync(int id)
    {
        return await Context.Customers
            .Include(c => c.Orders)
            .ThenInclude(o => o.OrderItems)
            .FirstOrDefaultAsync(c => c.Id == id);
    }
}