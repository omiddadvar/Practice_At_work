using Inventory.WebAPI.Abstractions.Services;
using Inventory.WebAPI.Models.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.WebAPI.Controllers;
[Route("api/[controller]")]
[ApiController]
public class StacksController : ControllerBase
{
    private readonly IStackService _stackService;

    public StacksController(IStackService stackService)
    {
        _stackService = stackService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<StackDto>>> GetStacks()
    {
        var stacks = await _stackService.GetStacksAsync();
        return Ok(stacks);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<StackDto>> GetStack(int id)
    {
        var stack = await _stackService.GetStackByIdAsync(id);
        if (stack == null)
        {
            return NotFound();
        }
        return Ok(stack);
    }

    [HttpGet("warehouse/{wareHouseId}")]
    public async Task<ActionResult<IEnumerable<StackDto>>> GetStacksByWareHouse(int wareHouseId)
    {
        var stacks = await _stackService.GetStacksByWareHouseAsync(wareHouseId);
        return Ok(stacks);
    }

    [HttpGet("product/{productId}")]
    public async Task<ActionResult<IEnumerable<StackDto>>> GetStacksByProduct(int productId)
    {
        var stacks = await _stackService.GetStacksByProductAsync(productId);
        return Ok(stacks);
    }

    [HttpPost]
    public async Task<ActionResult<StackDto>> CreateStack(CreateStackDto createStackDto)
    {
        try
        {
            var stack = await _stackService.CreateStackAsync(createStackDto);
            return CreatedAtAction(nameof(GetStack), new { id = stack.Id }, stack);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateStack(int id, UpdateStackDto updateStackDto)
    {
        try
        {
            var stack = await _stackService.UpdateStackAsync(id, updateStackDto);
            if (stack == null)
            {
                return NotFound();
            }
            return Ok(stack);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteStack(int id)
    {
        var result = await _stackService.DeleteStackAsync(id);
        if (!result)
        {
            return NotFound();
        }
        return NoContent();
    }
}
