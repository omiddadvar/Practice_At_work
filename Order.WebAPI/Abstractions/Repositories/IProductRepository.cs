using Order.WebAPI.Models.Entities;

namespace Order.WebAPI.Abstractions.Repositories;

public interface IProductRepository : IRepositoryBase<Product>
{
    Task<Product?> GetBySkuAsync(string sku);
    Task<bool> ProductExistsAsync(string sku);
    Task<IEnumerable<Product>> GetActiveProductsAsync();
}