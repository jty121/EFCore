using Infrastructure.Dtos;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Entities;

public class DescriptionEntity
{
    [Key]
    public int Id { get; set; }
    [Required]
    //anges inget blir det automatiskt nvarchar max
    public string Description { get; set; } = null!;

    [Required]
    [Column(TypeName = "nvarchar (200)")]
    public string Language { get; set; } = null!;

    [Required]
    [Column(TypeName = "nvarchar (200)")]
    public string Narrator { get; set; } = null!;

    [Required]
    [Column(TypeName = "nvarchar (200)")]
    public string? Illustrator { get; set; } //kan vara null, vissa böcker har inga bilder alls. 

    // navigering till BookEntity
    public int BookId { get; set; }
    public BookEntity Book { get; set; } = null!;

    public static implicit operator DescriptionEntity(DescriptionDto description)
    {
        return new DescriptionEntity
        {
            Description = description.Description,
            Language = description.Language,
            Narrator = description.Narrator,
            Illustrator = description.Illustrator,
            BookId = description.BookId,
        };
    }
}
