using Inventory.WebAPI.Models.DTOs;

namespace Inventory.WebAPI.Abstractions.Services;
public interface IWareHouseService
{
    Task<IEnumerable<WareHouseDto>> GetWareHousesAsync();
    Task<WareHouseDto?> GetWareHouseByIdAsync(int id);
    Task<WareHouseDto> CreateWareHouseAsync(CreateWareHouseDto createWareHouseDto);
    Task<WareHouseDto?> UpdateWareHouseAsync(int id, UpdateWareHouseDto updateWareHouseDto);
    Task<bool> DeleteWareHouseAsync(int id);
}