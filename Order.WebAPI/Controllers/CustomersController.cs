using Microsoft.AspNetCore.Mvc;
using Order.WebAPI.Abstractions.Services;
using Order.WebAPI.Models.DTOs;

namespace Order.WebAPI.Controllers;
[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    private readonly ICustomerService _customerService;

    public CustomersController(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CustomerDto>>> GetCustomers()
    {
        var customers = await _customerService.GetCustomersAsync();
        return Ok(customers);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CustomerDto>> GetCustomer(int id)
    {
        var customer = await _customerService.GetCustomerByIdAsync(id);
        if (customer == null)
        {
            return NotFound();
        }
        return Ok(customer);
    }

    [HttpGet("{id}/orders")]
    public async Task<ActionResult<IEnumerable<OrderDto>>> GetCustomerOrders(int id)
    {
        var orders = await _customerService.GetCustomerOrdersAsync(id);
        return Ok(orders);
    }

    [HttpPost]
    public async Task<ActionResult<CustomerDto>> CreateCustomer(CreateCustomerDto createCustomerDto)
    {
        try
        {
            var customer = await _customerService.CreateCustomerAsync(createCustomerDto);
            return CreatedAtAction(nameof(GetCustomer), new { id = customer.Id }, customer);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCustomer(int id, UpdateCustomerDto updateCustomerDto)
    {
        var customer = await _customerService.UpdateCustomerAsync(id, updateCustomerDto);
        if (customer == null)
        {
            return NotFound();
        }
        return Ok(customer);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCustomer(int id)
    {
        var result = await _customerService.DeleteCustomerAsync(id);
        if (!result)
        {
            return NotFound();
        }
        return NoContent();
    }
}