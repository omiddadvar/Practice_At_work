using AutoMapper;
using Grpc.Core;
using Inventory.WebAPI.Protos;
using Inventory.WebAPI.Abstractions.Services;
using Inventory.WebAPI.Models.DTOs;

namespace Inventory.WebAPI.Services;

public class InventoryGrpcService : InventoryServiceProto.InventoryServiceProtoBase
{
    private readonly IProductService _productService;
    private readonly ICategoryService _categoryService;
    private readonly ILogger<InventoryGrpcService> _logger;
    private readonly IMapper _mapper;

    public InventoryGrpcService(
        IProductService productService, 
        ICategoryService categoryService,
        ILogger<InventoryGrpcService> logger, 
        IMapper mapper)
    {
        _productService = productService;
        _categoryService = categoryService;
        _logger = logger;
        _mapper = mapper;
    }

    public override async Task<ProductResponse> GetProduct(GetProductRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Getting product via gRPC - ID: {ProductId}, SKU: {Sku}", request.Id, request.Sku);

        ProductDto product;

        if (request.Id > 0)
        {
            product = await _productService.GetProductByIdAsync(request.Id);
        }
        else if (!string.IsNullOrEmpty(request.Sku))
        {
            // You might need to add a method to get product by SKU in your service
            var products = await _productService.GetProductsAsync();
            product = products.FirstOrDefault(p => p.SKU == request.Sku);
        }
        else
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Either ID or SKU must be provided"));
        }

        if (product == null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, "Product not found"));
        }

        return _mapper.Map<ProductResponse>(product);
    }

    public override async Task<ProductsResponse> GetProducts(GetProductsRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Getting products via gRPC");

        var products = await _productService.GetProductsAsync();
        var productResponses = _mapper.Map<List<ProductResponse>>(products);

        return new ProductsResponse
        {
            Products = { productResponses },
            TotalCount = products.Count()
        };
    }

    public override async Task<CategoryResponse> GetCategory(GetCategoryRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Getting category via gRPC - ID: {CategoryId}", request.Id);

        var category = await _categoryService.GetCategoryByIdAsync(request.Id);

        if (category == null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, "Category not found"));
        }

        return _mapper.Map<CategoryResponse>(category);
    }

    public override async Task<CategoriesResponse> GetCategories(GetCategoriesRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Getting categories via gRPC");

        var categories = await _categoryService.GetCategoriesAsync();
        var categoryResponses = _mapper.Map<List<CategoryResponse>>(categories);

        return new CategoriesResponse
        {
            Categories = { categoryResponses },
            TotalCount = categories.Count()
        };
    }

    public override async Task<ProductsResponse> GetProductsByCategory(GetProductsByCategoryRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Getting products by category via gRPC - Category ID: {CategoryId}", request.CategoryId);

        var products = await _productService.GetProductsAsync();
        var categoryProducts = products.Where(p => p.CategoryId == request.CategoryId).ToList();
        var productResponses = _mapper.Map<List<ProductResponse>>(categoryProducts);

        return new ProductsResponse
        {
            Products = { productResponses },
            TotalCount = categoryProducts.Count
        };
    }
}