using Inventory.WebAPI.Models.Entities;

namespace Inventory.WebAPI.Abstractions.Repositories;

public interface IProductRepository : IRepositoryBase<Product>
{
    Task<IEnumerable<Product>> GetProductsWithCategoryAsync();
    Task<Product?> GetProductWithCategoryAsync(int id);
    Task<Product?> GetBySkuAsync(string sku);
    Task<bool> ProductExistsAsync(string sku);
}