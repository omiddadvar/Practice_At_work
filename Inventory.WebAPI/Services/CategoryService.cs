using AutoMapper;
using Inventory.WebAPI.Abstractions.Repositories;
using Inventory.WebAPI.Abstractions.Services;
using Inventory.WebAPI.Models.DTOs;
using Inventory.WebAPI.Models.Entities;

namespace Inventory.WebAPI.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper _mapper;

    public CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
    {
        _categoryRepository = categoryRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CategoryDto>> GetCategoriesAsync()
    {
        var categories = await _categoryRepository.FindAllAsync();
        return _mapper.Map<IEnumerable<CategoryDto>>(categories);
    }

    public async Task<CategoryDto?> GetCategoryByIdAsync(int id)
    {
        var category = await _categoryRepository.FindByIdAsync(id);
        return category == null ? null : _mapper.Map<CategoryDto>(category);
    }

    public async Task<CategoryDto> CreateCategoryAsync(CreateCategoryDto createCategoryDto)
    {
        if (await _categoryRepository.CategoryExistsAsync(createCategoryDto.Name))
        {
            throw new ArgumentException($"Category with name '{createCategoryDto.Name}' already exists.");
        }

        var category = _mapper.Map<Category>(createCategoryDto);
        var createdCategory = await _categoryRepository.CreateAsync(category);
        return _mapper.Map<CategoryDto>(createdCategory);
    }

    public async Task<CategoryDto?> UpdateCategoryAsync(int id, UpdateCategoryDto updateCategoryDto)
    {
        var category = await _categoryRepository.FindByIdAsync(id);
        if (category == null) return null;

        if (category.Name != updateCategoryDto.Name &&
            await _categoryRepository.CategoryExistsAsync(updateCategoryDto.Name))
        {
            throw new ArgumentException($"Category with name '{updateCategoryDto.Name}' already exists.");
        }

        _mapper.Map(updateCategoryDto, category);
        var updatedCategory = await _categoryRepository.UpdateAsync(category);
        return _mapper.Map<CategoryDto>(updatedCategory);
    }

    public async Task<bool> DeleteCategoryAsync(int id)
    {
        var category = await _categoryRepository.FindByIdAsync(id);
        if (category == null) return false;

        await _categoryRepository.DeleteAsync(category);
        return true;
    }
}