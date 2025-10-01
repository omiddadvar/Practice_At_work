using AutoMapper;
using Order.WebAPI.Abstractions.Repositories;
using Order.WebAPI.Abstractions.Services;
using Order.WebAPI.Models.DTOs;
using Order.WebAPI.Models.Entities;

namespace Order.WebAPI.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public ProductService(IProductRepository productRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ProductDto>> GetProductsAsync()
    {
        var products = await _productRepository.FindAllAsync();
        return _mapper.Map<IEnumerable<ProductDto>>(products);
    }

    public async Task<ProductDto?> GetProductByIdAsync(int id)
    {
        var product = await _productRepository.FindByIdAsync(id);
        return product == null ? null : _mapper.Map<ProductDto>(product);
    }

    public async Task<ProductDto?> GetProductBySkuAsync(string sku)
    {
        var product = await _productRepository.GetBySkuAsync(sku);
        return product == null ? null : _mapper.Map<ProductDto>(product);
    }

    public async Task<ProductDto> CreateProductAsync(CreateProductDto createProductDto)
    {
        if (await _productRepository.ProductExistsAsync(createProductDto.SKU))
        {
            throw new ArgumentException($"Product with SKU '{createProductDto.SKU}' already exists.");
        }

        var product = _mapper.Map<Product>(createProductDto);
        var createdProduct = await _productRepository.CreateAsync(product);
        return _mapper.Map<ProductDto>(createdProduct);
    }

    public async Task<ProductDto?> UpdateProductAsync(int id, UpdateProductDto updateProductDto)
    {
        var product = await _productRepository.FindByIdAsync(id);
        if (product == null) return null;

        if (product.SKU != updateProductDto.SKU &&
            await _productRepository.ProductExistsAsync(updateProductDto.SKU)) _mapper.Map(updateProductDto, product);
        var updatedProduct = await _productRepository.UpdateAsync(product);
        return _mapper.Map<ProductDto>(updatedProduct);
    }

    public async Task<bool> DeleteProductAsync(int id)
    {
        var product = await _productRepository.FindByIdAsync(id);
        if (product == null) return false;

        await _productRepository.DeleteAsync(product);
        return true;
    }

    public async Task<bool> DeactivateProductAsync(int id)
    {
        var product = await _productRepository.FindByIdAsync(id);
        if (product == null) return false;

        product.IsActive = false;
        await _productRepository.UpdateAsync(product);
        return true;
    }

    public async Task<bool> ActivateProductAsync(int id)
    {
        var product = await _productRepository.FindByIdAsync(id);
        if (product == null) return false;

        product.IsActive = true;
        await _productRepository.UpdateAsync(product);
        return true;
    }
}