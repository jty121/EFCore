using Infrastructure.Dtos;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Entities;

public class CategoryEntity
{
    [Key]
    public int Id { get; set; }
    [Required]
    [Column(TypeName = "nvarchar (200)")]
    public string CategoryName { get; set; } = null!;

    //navigering till BookEntity, relation MÅNGA-till en 
    public ICollection<BookEntity> Book { get; set; } = new HashSet<BookEntity>();

    public static implicit operator CategoryEntity(CategoryDto category)
    {
        return new CategoryEntity
        {
            Id = category.Id,
            CategoryName = category.CategoryName,
        };
    }
}
