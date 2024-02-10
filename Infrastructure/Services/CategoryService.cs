
using Infrastructure.Dtos;
using Infrastructure.Entities;
using Infrastructure.Repositories;
using System.Diagnostics;

namespace Infrastructure.Services;

public class CategoryService(CategoryRepository categoryRepository)
{
    private readonly CategoryRepository _categoryRepository = categoryRepository;

    public async Task<CategoryEntity> CreateCategoryAsync(string categoryName)
    {
        try
        {
            if (!await _categoryRepository.ExistingAsync(x => x.CategoryName == categoryName))
            {
                var categoryEntity = await _categoryRepository.CreateAsync(new CategoryEntity { CategoryName = categoryName });
                if(categoryEntity != null)
                {
                    return categoryEntity;
                }
            }
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }
        return null!;
    }

    public async Task<CategoryDto> GetCategoryAsync(int id)
    {
        try
        {
            var categoryEntity = await _categoryRepository.GetAsync(x => x.Id == id);
            if (categoryEntity != null)
            {
                var categoryDto = new CategoryDto { Id = categoryEntity.Id, CategoryName = categoryEntity.CategoryName };
                return categoryDto;
            }
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }
        return null!;
    }

    public async Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync()
    {
        try
        {
            var categoryEntities = await _categoryRepository.GetAllAsync();
            if(categoryEntities != null)
            {
                var categoryList = new List<CategoryDto>();

                foreach (var categoryEntity in categoryEntities)
                {
                    categoryList.Add(new CategoryDto
                    {
                        Id = categoryEntity.Id,
                        CategoryName = categoryEntity.CategoryName,
                    });
                }
                return categoryList;
            }
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }
        return null!;
    }
    
    public async Task<CategoryDto> UpdateCategoryAsync(CategoryDto updatedCategory)
    {
        try
        {
            var categoryEntity = await _categoryRepository.GetAsync(x => x.Id == updatedCategory.Id);
            if (categoryEntity != null)
            {
                categoryEntity.Id = updatedCategory.Id;
                var updatedCategoryEntity = await _categoryRepository.UpdateOneAsync(categoryEntity);
                if (updatedCategoryEntity != null)
                {
                    var newCategory = new CategoryDto { Id = updatedCategory.Id, CategoryName = updatedCategory.CategoryName };
                    return newCategory;
                }
            }
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }
        return null!;
    }

    public async Task<bool> DeleteCategoryAsync(int id)
    {
        try
        {
            var result = await _categoryRepository.DeleteAsync(x => x.Id == id);
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }
        return false;
    }
}
