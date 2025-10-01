using Order.WebAPI.Models.DTOs;

namespace Order.WebAPI.Abstractions.Services;

public interface IInventoryGrpcClient
{
    Task<ProductDto?> GetProductAsync(int id, string sku = "");
    Task<IEnumerable<ProductDto>> GetProductsAsync();
    Task<CategoryDto?> GetCategoryAsync(int id);
    Task<IEnumerable<CategoryDto>> GetCategoriesAsync();
    Task<IEnumerable<ProductDto>> GetProductsByCategoryAsync(int categoryId);
    Task<bool> ValidateProductAsync(int productId, string sku);
    Task<ProductDto?> GetProductByIdAsync(int productId);
    Task<ProductDto?> GetProductBySkuAsync(string sku);
}