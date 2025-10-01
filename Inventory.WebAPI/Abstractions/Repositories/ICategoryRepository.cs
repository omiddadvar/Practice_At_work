using Inventory.WebAPI.Models.Entities;

namespace Inventory.WebAPI.Abstractions.Repositories;

public interface ICategoryRepository : IRepositoryBase<Category>
{
    Task<Category?> GetByNameAsync(string name);
    Task<bool> CategoryExistsAsync(string name);
}