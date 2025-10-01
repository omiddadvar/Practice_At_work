using Inventory.WebAPI.Abstractions.Repositories;
using Inventory.WebAPI.Data;
using Inventory.WebAPI.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Inventory.WebAPI.Repositories;
public class ProductRepository : RepositoryBase<Product>, IProductRepository
{
    public ProductRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Product>> GetProductsWithCategoryAsync()
    {
        return await Context.Products
            .Include(p => p.Category)
            .ToListAsync();
    }

    public async Task<Product?> GetProductWithCategoryAsync(int id)
    {
        return await Context.Products
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Product?> GetBySkuAsync(string sku)
    {
        return await Context.Products
            .FirstOrDefaultAsync(p => p.SKU.ToLower() == sku.ToLower());
    }

    public async Task<bool> ProductExistsAsync(string sku)
    {
        return await Context.Products
            .AnyAsync(p => p.SKU.ToLower() == sku.ToLower());
    }
}