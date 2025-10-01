using AutoMapper;
using Inventory.WebAPI.Abstractions.Repositories;
using Inventory.WebAPI.Abstractions.Services;
using Inventory.WebAPI.Models.DTOs;
using Inventory.WebAPI.Models.Entities;

namespace Inventory.WebAPI.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper _mapper;

    public ProductService(IProductRepository productRepository, ICategoryRepository categoryRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ProductDto>> GetProductsAsync()
    {
        var products = await _productRepository.GetProductsWithCategoryAsync();
        return _mapper.Map<IEnumerable<ProductDto>>(products);
    }

    public async Task<ProductDto?> GetProductByIdAsync(int id)
    {
        var product = await _productRepository.GetProductWithCategoryAsync(id);
        return product == null ? null : _mapper.Map<ProductDto>(product);
    }

    public async Task<ProductDto> CreateProductAsync(CreateProductDto createProductDto)
    {
        if (await _productRepository.ProductExistsAsync(createProductDto.SKU))
        {
            throw new ArgumentException($"Product with SKU '{createProductDto.SKU}' already exists.");
        }

        if (!await _categoryRepository.ExistsAsync(createProductDto.CategoryId))
        {
            throw new ArgumentException($"Category with ID '{createProductDto.CategoryId}' does not exist.");
        }

        var product = _mapper.Map<Product>(createProductDto);
        var createdProduct = await _productRepository.CreateAsync(product);

        // Reload with category to get category name
        var productWithCategory = await _productRepository.GetProductWithCategoryAsync(createdProduct.Id);
        return _mapper.Map<ProductDto>(productWithCategory!);
    }

    public async Task<ProductDto?> UpdateProductAsync(int id, UpdateProductDto updateProductDto)
    {
        var product = await _productRepository.FindByIdAsync(id);
        if (product == null) return null;

        if (product.SKU != updateProductDto.SKU &&
            await _productRepository.ProductExistsAsync(updateProductDto.SKU))
        {
            throw new ArgumentException($"Product with SKU '{updateProductDto.SKU}' already exists.");
        }

        if (!await _categoryRepository.ExistsAsync(updateProductDto.CategoryId))
        {
            throw new ArgumentException($"Category with ID '{updateProductDto.CategoryId}' does not exist.");
        }

        _mapper.Map(updateProductDto, product);
        var updatedProduct = await _productRepository.UpdateAsync(product);

        // Reload with category to get category name
        var productWithCategory = await _productRepository.GetProductWithCategoryAsync(updatedProduct.Id);
        return _mapper.Map<ProductDto>(productWithCategory!);
    }

    public async Task<bool> DeleteProductAsync(int id)
    {
        var product = await _productRepository.FindByIdAsync(id);
        if (product == null) return false;

        await _productRepository.DeleteAsync(product);
        return true;
    }
}