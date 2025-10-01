using Inventory.WebAPI.Abstractions.Repositories;
using Inventory.WebAPI.Data;
using Inventory.WebAPI.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Inventory.WebAPI.Repositories;

public class StackRepository : RepositoryBase<Stack>, IStackRepository
{
    public StackRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Stack>> GetStacksWithDetailsAsync()
    {
        return await Context.Stacks
            .Include(s => s.Product)
            .Include(s => s.WareHouse)
            .ToListAsync();
    }

    public async Task<Stack?> GetStackWithDetailsAsync(int id)
    {
        return await Context.Stacks
            .Include(s => s.Product)
            .Include(s => s.WareHouse)
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<Stack?> GetByProductAndWareHouseAsync(int productId, int wareHouseId)
    {
        return await Context.Stacks
            .FirstOrDefaultAsync(s => s.ProductId == productId && s.WareHouseId == wareHouseId);
    }

    public async Task<IEnumerable<Stack>> GetStacksByWareHouseAsync(int wareHouseId)
    {
        return await Context.Stacks
            .Include(s => s.Product)
            .Where(s => s.WareHouseId == wareHouseId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Stack>> GetStacksByProductAsync(int productId)
    {
        return await Context.Stacks
            .Include(s => s.WareHouse)
            .Where(s => s.ProductId == productId)
            .ToListAsync();
    }
}