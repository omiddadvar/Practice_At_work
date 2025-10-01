using Inventory.WebAPI.Abstractions.Repositories;
using Inventory.WebAPI.Data;
using Inventory.WebAPI.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Inventory.WebAPI.Repositories;

public class WareHouseRepository : RepositoryBase<WareHouse>, IWareHouseRepository
{
    public WareHouseRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<WareHouse?> GetByNameAsync(string name)
    {
        return await Context.WareHouses
            .FirstOrDefaultAsync(w => w.Name.ToLower() == name.ToLower());
    }

    public async Task<bool> WareHouseExistsAsync(string name)
    {
        return await Context.WareHouses
            .AnyAsync(w => w.Name.ToLower() == name.ToLower());
    }

    public async Task<IEnumerable<WareHouse>> GetWareHousesWithStacksAsync()
    {
        return await Context.WareHouses
            .Include(w => w.Stacks)
            .ThenInclude(s => s.Product)
            .ToListAsync();
    }
}