using Inventory.WebAPI.Abstractions.Repositories;
using Inventory.WebAPI.Data;
using Inventory.WebAPI.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Inventory.WebAPI.Repositories;

public class CategoryRepository : RepositoryBase<Category>, ICategoryRepository
{
    public CategoryRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Category?> GetByNameAsync(string name)
    {
        return await Context.Categories
            .FirstOrDefaultAsync(c => c.Name.ToLower() == name.ToLower());
    }

    public async Task<bool> CategoryExistsAsync(string name)
    {
        return await Context.Categories
            .AnyAsync(c => c.Name.ToLower() == name.ToLower());
    }
}