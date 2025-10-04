using Microsoft.AspNetCore.Mvc;
using Order.WebAPI.Abstractions.Services;
using Order.WebAPI.Models.DTOs;

namespace Order.WebAPI.Controllers;
[Route("api/[controller]")]
[ApiController]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrdersController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrders()
    {
        var orders = await _orderService.GetOrdersAsync();
        return Ok(orders);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<OrderDto>> GetOrder(int id)
    {
        var order = await _orderService.GetOrderByIdAsync(id);
        if (order == null)
        {
            return NotFound();
        }
        return Ok(order);
    }

    [HttpGet("number/{orderNumber}")]
    public async Task<ActionResult<OrderDto>> GetOrderByNumber(string orderNumber)
    {
        var order = await _orderService.GetOrderByNumberAsync(orderNumber);
        if (order == null)
        {
            return NotFound();
        }
        return Ok(order);
    }

    [HttpGet("status/{status}")]
    public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrdersByStatus(string status)
    {
        var orders = await _orderService.GetOrdersByStatusAsync(status);
        return Ok(orders);
    }

    [HttpGet("date-range")]
    public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrdersByDateRange(
        [FromQuery] DateTime startDate,
        [FromQuery] DateTime endDate)
    {
        var orders = await _orderService.GetOrdersByDateRangeAsync(startDate, endDate);
        return Ok(orders);
    }

    [HttpPost]
    public async Task<ActionResult<OrderDto>> CreateOrder(CreateOrderDto createOrderDto)
    {
        try
        {
            var order = await _orderService.CreateOrderAsync(createOrderDto);
            return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, order);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateOrder(int id, UpdateOrderDto updateOrderDto)
    {
        try
        {
            var order = await _orderService.UpdateOrderAsync(id, updateOrderDto);
            if (order == null)
            {
                return NotFound();
            }
            return Ok(order);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPatch("{id}/status")]
    public async Task<IActionResult> UpdateOrderStatus(int id, UpdateOrderStatusDto updateOrderStatusDto)
    {
        try
        {
            var order = await _orderService.UpdateOrderStatusAsync(id, updateOrderStatusDto);
            if (order == null)
            {
                return NotFound();
            }
            return Ok(order);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteOrder(int id)
    {
        var result = await _orderService.DeleteOrderAsync(id);
        if (!result)
        {
            return NotFound();
        }
        return NoContent();
    }
}