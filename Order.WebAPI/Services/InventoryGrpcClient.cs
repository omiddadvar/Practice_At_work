using Grpc.Core;
using Grpc.Net.Client;
using Order.WebAPI.Abstractions.Services;
using Order.WebAPI.Protos;
using AutoMapper;
using Order.WebAPI.Models.DTOs;

namespace Order.WebAPI.Services;

public class InventoryGrpcClient : IInventoryGrpcClient, IDisposable
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<InventoryGrpcClient> _logger;
    private readonly IMapper _mapper;
    private GrpcChannel? _channel;
    private InventoryService.InventoryServiceClient? _client;

    public InventoryGrpcClient(IConfiguration configuration, ILogger<InventoryGrpcClient> logger, IMapper mapper)
    {
        _configuration = configuration;
        _logger = logger;
        _mapper = mapper;
    }

    private InventoryService.InventoryServiceClient GetClient()
    {
        if (_client == null)
        {
            var inventoryServiceUrl = _configuration["InventoryService:Url"] ?? "https://localhost:7001";
            _channel = GrpcChannel.ForAddress(inventoryServiceUrl);
            _client = new InventoryService.InventoryServiceClient(_channel);
        }
        return _client;
    }

    public async Task<ProductDto?> GetProductAsync(int id, string sku = "")
    {
        try
        {
            var request = new GetProductRequest { Id = id, Sku = sku ?? "" };
            var response = await GetClient().GetProductAsync(request);
            return _mapper.Map<ProductDto>(response);
        }
        catch (RpcException ex) when (ex.StatusCode == StatusCode.NotFound)
        {
            _logger.LogWarning("Product not found via gRPC - ID: {ProductId}, SKU: {Sku}", id, sku);
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting product via gRPC - ID: {ProductId}, SKU: {Sku}", id, sku);
            throw;
        }
    }

    public async Task<IEnumerable<ProductDto>> GetProductsAsync()
    {
        try
        {
            var request = new GetProductsRequest();
            var response = await GetClient().GetProductsAsync(request);
            return _mapper.Map<IEnumerable<ProductDto>>(response.Products);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting products via gRPC");
            throw;
        }
    }

    public async Task<CategoryDto?> GetCategoryAsync(int id)
    {
        try
        {
            var request = new GetCategoryRequest { Id = id };
            var response = await GetClient().GetCategoryAsync(request);
            return _mapper.Map<CategoryDto>(response);
        }
        catch (RpcException ex) when (ex.StatusCode == StatusCode.NotFound)
        {
            _logger.LogWarning("Category not found via gRPC - ID: {CategoryId}", id);
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting category via gRPC - ID: {CategoryId}", id);
            throw;
        }
    }

    public async Task<IEnumerable<CategoryDto>> GetCategoriesAsync()
    {
        try
        {
            var request = new GetCategoriesRequest();
            var response = await GetClient().GetCategoriesAsync(request);
            return _mapper.Map<IEnumerable<CategoryDto>>(response.Categories);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting categories via gRPC");
            throw;
        }
    }

    public async Task<IEnumerable<ProductDto>> GetProductsByCategoryAsync(int categoryId)
    {
        try
        {
            var request = new GetProductsByCategoryRequest { CategoryId = categoryId };
            var response = await GetClient().GetProductsByCategoryAsync(request);
            return _mapper.Map<IEnumerable<ProductDto>>(response.Products);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting products by category via gRPC - Category ID: {CategoryId}", categoryId);
            throw;
        }
    }
    public async Task<bool> ValidateProductAsync(int productId, string sku)
    {
        try
        {
            var product = await GetProductAsync(productId, sku);
            return product != null;
        }
        catch
        {
            return false;
        }
    }
    public async Task<ProductDto?> GetProductByIdAsync(int productId)
    {
        return await GetProductAsync(productId, "");
    }

    public async Task<ProductDto?> GetProductBySkuAsync(string sku)
    {
        return await GetProductAsync(0, sku);
    }
    public void Dispose()
    {
        _channel?.Dispose();
    }
}