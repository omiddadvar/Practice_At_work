using Order.WebAPI.Models.DTOs;

namespace Order.WebAPI.Abstractions.Services;

public interface IProductService
{
    Task<IEnumerable<ProductDto>> GetProductsAsync();
    Task<ProductDto?> GetProductByIdAsync(int id);
    Task<ProductDto?> GetProductBySkuAsync(string sku);
    Task<ProductDto> CreateProductAsync(CreateProductDto createProductDto);
    Task<ProductDto?> UpdateProductAsync(int id, UpdateProductDto updateProductDto);
    Task<bool> DeleteProductAsync(int id);
    Task<bool> DeactivateProductAsync(int id);
    Task<bool> ActivateProductAsync(int id);
}