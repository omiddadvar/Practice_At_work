using Inventory.WebAPI.Models.DTOs;

namespace Inventory.WebAPI.Abstractions.Services;

public interface IStackService
{
    Task<IEnumerable<StackDto>> GetStacksAsync();
    Task<StackDto?> GetStackByIdAsync(int id);
    Task<StackDto> CreateStackAsync(CreateStackDto createStackDto);
    Task<StackDto?> UpdateStackAsync(int id, UpdateStackDto updateStackDto);
    Task<bool> DeleteStackAsync(int id);
    Task<IEnumerable<StackDto>> GetStacksByWareHouseAsync(int wareHouseId);
    Task<IEnumerable<StackDto>> GetStacksByProductAsync(int productId);
}