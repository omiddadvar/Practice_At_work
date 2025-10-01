using Inventory.WebAPI.Abstractions.Services;
using Inventory.WebAPI.Models.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.WebAPI.Controllers;
[Route("api/[controller]")]
[ApiController]
public class WareHousesController : ControllerBase
{
    private readonly IWareHouseService _wareHouseService;

    public WareHousesController(IWareHouseService wareHouseService)
    {
        _wareHouseService = wareHouseService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<WareHouseDto>>> GetWareHouses()
    {
        var wareHouses = await _wareHouseService.GetWareHousesAsync();
        return Ok(wareHouses);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<WareHouseDto>> GetWareHouse(int id)
    {
        var wareHouse = await _wareHouseService.GetWareHouseByIdAsync(id);
        if (wareHouse == null)
        {
            return NotFound();
        }
        return Ok(wareHouse);
    }

    [HttpPost]
    public async Task<ActionResult<WareHouseDto>> CreateWareHouse(CreateWareHouseDto createWareHouseDto)
    {
        try
        {
            var wareHouse = await _wareHouseService.CreateWareHouseAsync(createWareHouseDto);
            return CreatedAtAction(nameof(GetWareHouse), new { id = wareHouse.Id }, wareHouse);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateWareHouse(int id, UpdateWareHouseDto updateWareHouseDto)
    {
        try
        {
            var wareHouse = await _wareHouseService.UpdateWareHouseAsync(id, updateWareHouseDto);
            if (wareHouse == null)
            {
                return NotFound();
            }
            return Ok(wareHouse);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteWareHouse(int id)
    {
        var result = await _wareHouseService.DeleteWareHouseAsync(id);
        if (!result)
        {
            return NotFound();
        }
        return NoContent();
    }
}
