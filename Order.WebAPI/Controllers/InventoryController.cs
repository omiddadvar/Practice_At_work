using Microsoft.AspNetCore.Mvc;
using Order.WebAPI.Abstractions.Services;
using Order.WebAPI.Models.DTOs;

namespace Order.WebAPI.Controllers;
[Route("api/[controller]")]
[ApiController]
public class InventoryController(IInventoryGrpcClient inventoryGrpcClient) : ControllerBase
{
    [HttpGet("products/{id}")]
    public async Task<ActionResult<ProductDto>> GetProduct(int id)
    {
        var product = await inventoryGrpcClient.GetProductAsync(id);
        if (product == null)
        {
            return NotFound();
        }
        return Ok(product);
    }

    [HttpGet("products/sku/{sku}")]
    public async Task<ActionResult<ProductDto>> GetProductBySku(string sku)
    {
        var product = await inventoryGrpcClient.GetProductBySkuAsync(sku);
        if (product == null)
        {
            return NotFound();
        }
        return Ok(product);
    }

    [HttpGet("products/{id}/validate")]
    public async Task<ActionResult<bool>> ValidateProduct(int id, [FromQuery] string? sku = null)
    {
        var isValid = await inventoryGrpcClient.ValidateProductAsync(id, sku ?? "");
        return Ok(isValid);
    }
}
