using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Order.WebAPI.Abstractions.Services;
using Order.WebAPI.Models.DTOs;

namespace Order.WebAPI.Controllers;
[Route("api/[controller]")]
[ApiController]
public class OrderItemsController : ControllerBase
{
    private readonly IOrderItemService _orderItemService;

    public OrderItemsController(IOrderItemService orderItemService)
    {
        _orderItemService = orderItemService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<OrderItemDto>>> GetOrderItems()
    {
        var orderItems = await _orderItemService.GetOrderItemsAsync();
        return Ok(orderItems);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<OrderItemDto>> GetOrderItem(int id)
    {
        var orderItem = await _orderItemService.GetOrderItemByIdAsync(id);
        if (orderItem == null)
        {
            return NotFound();
        }
        return Ok(orderItem);
    }

    [HttpGet("order/{orderId}")]
    public async Task<ActionResult<IEnumerable<OrderItemDto>>> GetOrderItemsByOrder(int orderId)
    {
        var orderItems = await _orderItemService.GetOrderItemsByOrderAsync(orderId);
        return Ok(orderItems);
    }

    [HttpGet("product/{productSku}")]
    public async Task<ActionResult<IEnumerable<OrderItemDto>>> GetOrderItemsByProduct(string productSku)
    {
        var orderItems = await _orderItemService.GetOrderItemsByProductAsync(productSku);
        return Ok(orderItems);
    }

    [HttpPost]
    public async Task<ActionResult<OrderItemDto>> CreateOrderItem(CreateOrderItemDto createOrderItemDto)
    {
        try
        {
            var orderItem = await _orderItemService.CreateOrderItemAsync(createOrderItemDto);
            return CreatedAtAction(nameof(GetOrderItem), new { id = orderItem.Id }, orderItem);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateOrderItem(int id, UpdateOrderItemDto updateOrderItemDto)
    {
        var orderItem = await _orderItemService.UpdateOrderItemAsync(id, updateOrderItemDto);
        if (orderItem == null)
        {
            return NotFound();
        }
        return Ok(orderItem);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteOrderItem(int id)
    {
        var result = await _orderItemService.DeleteOrderItemAsync(id);
        if (!result)
        {
            return NotFound();
        }
        return NoContent();
    }
}