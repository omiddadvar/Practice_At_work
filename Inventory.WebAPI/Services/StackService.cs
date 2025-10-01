using AutoMapper;
using Inventory.WebAPI.Abstractions.Repositories;
using Inventory.WebAPI.Abstractions.Services;
using Inventory.WebAPI.Models.DTOs;

namespace Inventory.WebAPI.Services;

public class StackService : IStackService
{
    private readonly IStackRepository _stackRepository;
    private readonly IProductRepository _productRepository;
    private readonly IWareHouseRepository _wareHouseRepository;
    private readonly IMapper _mapper;

    public StackService(IStackRepository stackRepository, IProductRepository productRepository,
                      IWareHouseRepository wareHouseRepository, IMapper mapper)
    {
        _stackRepository = stackRepository;
        _productRepository = productRepository;
        _wareHouseRepository = wareHouseRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<StackDto>> GetStacksAsync()
    {
        var stacks = await _stackRepository.GetStacksWithDetailsAsync();
        return _mapper.Map<IEnumerable<StackDto>>(stacks);
    }

    public async Task<StackDto?> GetStackByIdAsync(int id)
    {
        var stack = await _stackRepository.GetStackWithDetailsAsync(id);
        return stack == null ? null : _mapper.Map<StackDto>(stack);
    }

    public async Task<StackDto> CreateStackAsync(CreateStackDto createStackDto)
    {
        // Check if product exists
        if (!await _productRepository.ExistsAsync(createStackDto.ProductId))
        {
            throw new ArgumentException($"Product with ID '{createStackDto.ProductId}' does not exist.");
        }

        // Check if warehouse exists
        if (!await _wareHouseRepository.ExistsAsync(createStackDto.WareHouseId))
        {
            throw new ArgumentException($"Warehouse with ID '{createStackDto.WareHouseId}' does not exist.");
        }

        // Check if stack already exists for this product in this warehouse
        var existingStack = await _stackRepository.GetByProductAndWareHouseAsync(
            createStackDto.ProductId, createStackDto.WareHouseId);

        if (existingStack != null)
        {
            throw new ArgumentException($"Stack for product ID '{createStackDto.ProductId}' in warehouse ID '{createStackDto.WareHouseId}' already exists.");
        }

        var stack = _mapper.Map<Stack>(createStackDto);
        var createdStack = await _stackRepository.CreateAsync(stack);

        // Reload with details
        var stackWithDetails = await _stackRepository.GetStackWithDetailsAsync(createdStack.Id);
        return _mapper.Map<StackDto>(stackWithDetails!);
    }

    public async Task<StackDto?> UpdateStackAsync(int id, UpdateStackDto updateStackDto)
    {
        var stack = await _stackRepository.FindByIdAsync(id);
        if (stack == null) return null;

        _mapper.Map(updateStackDto, stack);
        var updatedStack = await _stackRepository.UpdateAsync(stack);

        // Reload with details
        var stackWithDetails = await _stackRepository.GetStackWithDetailsAsync(updatedStack.Id);
        return _mapper.Map<StackDto>(stackWithDetails!);
    }

    public async Task<bool> DeleteStackAsync(int id)
    {
        var stack = await _stackRepository.FindByIdAsync(id);
        if (stack == null) return false;

        await _stackRepository.DeleteAsync(stack);
        return true;
    }

    public async Task<IEnumerable<StackDto>> GetStacksByWareHouseAsync(int wareHouseId)
    {
        var stacks = await _stackRepository.GetStacksByWareHouseAsync(wareHouseId);
        return _mapper.Map<IEnumerable<StackDto>>(stacks);
    }

    public async Task<IEnumerable<StackDto>> GetStacksByProductAsync(int productId)
    {
        var stacks = await _stackRepository.GetStacksByProductAsync(productId);
        return _mapper.Map<IEnumerable<StackDto>>(stacks);
    }
}