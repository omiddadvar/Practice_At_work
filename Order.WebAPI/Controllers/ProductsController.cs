using Microsoft.AspNetCore.Mvc;
using Order.WebAPI.Abstractions.Services;
using Order.WebAPI.Models.DTOs;

namespace Order.WebAPI.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts()
    {
        var products = await _productService.GetProductsAsync();
        return Ok(products);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductDto>> GetProduct(int id)
    {
        var product = await _productService.GetProductByIdAsync(id);
        if (product == null)
        {
            return NotFound();
        }
        return Ok(product);
    }

    [HttpGet("sku/{sku}")]
    public async Task<ActionResult<ProductDto>> GetProductBySku(string sku)
    {
        var product = await _productService.GetProductBySkuAsync(sku);
        if (product == null)
        {
            return NotFound();
        }
        return Ok(product);
    }

    [HttpPost]
    public async Task<ActionResult<ProductDto>> CreateProduct(CreateProductDto createProductDto)
    {
        try
        {
            var product = await _productService.CreateProductAsync(createProductDto);
            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct(int id, UpdateProductDto updateProductDto)
    {
        var product = await _productService.UpdateProductAsync(id, updateProductDto);
        if (product == null)
        {
            return NotFound();
        }
        return Ok(product);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var result = await _productService.DeleteProductAsync(id);
        if (!result)
        {
            return NotFound();
        }
        return NoContent();
    }

    [HttpPatch("{id}/deactivate")]
    public async Task<IActionResult> DeactivateProduct(int id)
    {
        var result = await _productService.DeactivateProductAsync(id);
        if (!result)
        {
            return NotFound();
        }
        return NoContent();
    }

    [HttpPatch("{id}/activate")]
    public async Task<IActionResult> ActivateProduct(int id)
    {
        var result = await _productService.ActivateProductAsync(id);
        if (!result)
        {
            return NotFound();
        }
        return NoContent();
    }
}