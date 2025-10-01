using Microsoft.EntityFrameworkCore;
using Order.WebAPI.Abstractions.Repositories;
using Order.WebAPI.Data;
using Order.WebAPI.Models.Entities;

namespace Order.WebAPI.Repositories;

public class ProductRepository : RepositoryBase<Product>, IProductRepository
{
    public ProductRepository(ApplicationDbContext context) : base(context)
    {
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

    public async Task<IEnumerable<Product>> GetActiveProductsAsync()
    {
        return await Context.Products
            .Where(p => p.IsActive)
            .ToListAsync();
    }
}