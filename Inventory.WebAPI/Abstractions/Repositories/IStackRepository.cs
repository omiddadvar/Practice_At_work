using Inventory.WebAPI.Models.Entities;

namespace Inventory.WebAPI.Abstractions.Repositories;

public interface IStackRepository : IRepositoryBase<Stack>
{
    Task<IEnumerable<Stack>> GetStacksWithDetailsAsync();
    Task<Stack?> GetStackWithDetailsAsync(int id);
    Task<Stack?> GetByProductAndWareHouseAsync(int productId, int wareHouseId);
    Task<IEnumerable<Stack>> GetStacksByWareHouseAsync(int wareHouseId);
    Task<IEnumerable<Stack>> GetStacksByProductAsync(int productId);
}