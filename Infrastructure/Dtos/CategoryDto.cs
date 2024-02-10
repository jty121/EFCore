using Infrastructure.Entities;
namespace Infrastructure.Dtos;

public class CategoryDto
{
    public int Id { get; set; }
    public string CategoryName { get; set; } = null!;


    public static implicit operator CategoryDto(CategoryEntity category)
    {
        var categoryDto = new CategoryDto
        {
            Id = category.Id,
            CategoryName = category.CategoryName,
        };
        return categoryDto;
    }
}
