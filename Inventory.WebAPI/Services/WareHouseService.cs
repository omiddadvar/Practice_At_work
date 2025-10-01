using AutoMapper;
using Inventory.WebAPI.Abstractions.Repositories;
using Inventory.WebAPI.Abstractions.Services;
using Inventory.WebAPI.Models.DTOs;
using Inventory.WebAPI.Models.Entities;

namespace Inventory.WebAPI.Services;

public class WareHouseService : IWareHouseService
{
    private readonly IWareHouseRepository _wareHouseRepository;
    private readonly IMapper _mapper;

    public WareHouseService(IWareHouseRepository wareHouseRepository, IMapper mapper)
    {
        _wareHouseRepository = wareHouseRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<WareHouseDto>> GetWareHousesAsync()
    {
        var wareHouses = await _wareHouseRepository.GetWareHousesWithStacksAsync();
        var wareHouseDtos = _mapper.Map<IEnumerable<WareHouseDto>>(wareHouses);

        // Calculate current stock for each warehouse
        foreach (var wareHouseDto in wareHouseDtos)
        {
            var wareHouse = wareHouses.First(w => w.Id == wareHouseDto.Id);
            wareHouseDto.CurrentStock = wareHouse.Stacks.Sum(s => s.Quantity);
        }

        return wareHouseDtos;
    }

    public async Task<WareHouseDto?> GetWareHouseByIdAsync(int id)
    {
        var wareHouse = await _wareHouseRepository.FindByIdAsync(id);
        if (wareHouse == null) return null;

        var wareHouseDto = _mapper.Map<WareHouseDto>(wareHouse);

        // Calculate current stock
        var stacks = await _wareHouseRepository.GetWareHousesWithStacksAsync();
        var currentWareHouse = stacks.FirstOrDefault(w => w.Id == id);
        wareHouseDto.CurrentStock = currentWareHouse?.Stacks.Sum(s => s.Quantity) ?? 0;

        return wareHouseDto;
    }

    public async Task<WareHouseDto> CreateWareHouseAsync(CreateWareHouseDto createWareHouseDto)
    {
        if (await _wareHouseRepository.WareHouseExistsAsync(createWareHouseDto.Name))
        {
            throw new ArgumentException($"Warehouse with name '{createWareHouseDto.Name}' already exists.");
        }

        var wareHouse = _mapper.Map<WareHouse>(createWareHouseDto);
        var createdWareHouse = await _wareHouseRepository.CreateAsync(wareHouse);
        return _mapper.Map<WareHouseDto>(createdWareHouse);
    }

    public async Task<WareHouseDto?> UpdateWareHouseAsync(int id, UpdateWareHouseDto updateWareHouseDto)
    {
        var wareHouse = await _wareHouseRepository.FindByIdAsync(id);
        if (wareHouse == null) return null;

        if (wareHouse.Name != updateWareHouseDto.Name &&
            await _wareHouseRepository.WareHouseExistsAsync(updateWareHouseDto.Name))
        {
            throw new ArgumentException($"Warehouse with name '{updateWareHouseDto.Name}' already exists.");
        }

        _mapper.Map(updateWareHouseDto, wareHouse);
        var updatedWareHouse = await _wareHouseRepository.UpdateAsync(wareHouse);
        return _mapper.Map<WareHouseDto>(updatedWareHouse);
    }

    public async Task<bool> DeleteWareHouseAsync(int id)
    {
        var wareHouse = await _wareHouseRepository.FindByIdAsync(id);
        if (wareHouse == null) return false;

        await _wareHouseRepository.DeleteAsync(wareHouse);
        return true;
    }
}