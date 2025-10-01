using Inventory.WebAPI.Models.Entities;

namespace Inventory.WebAPI.Abstractions.Repositories;

public interface IWareHouseRepository : IRepositoryBase<WareHouse>
{
    Task<WareHouse?> GetByNameAsync(string name);
    Task<bool> WareHouseExistsAsync(string name);
    Task<IEnumerable<WareHouse>> GetWareHousesWithStacksAsync();
}