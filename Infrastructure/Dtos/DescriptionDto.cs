using Infrastructure.Entities;
namespace Infrastructure.Dtos;

public class DescriptionDto 
{
    public int Id { get; set; }
    public string Description { get; set; } = null!;
    public string Language { get; set; } = null!;
    public string Narrator { get; set; } = null!;
    public string? Illustrator { get; set; }
    public int BookId { get; set; }
    public string ISBN { get; set; } = null!;

    public static implicit operator DescriptionDto(DescriptionEntity description)
    {
        var descriptionDto = new DescriptionDto
        {
            Id = description.Id,
            Description = description.Description,
            Language = description.Language,
            Narrator = description.Narrator,
            Illustrator = description.Illustrator,
            BookId = description.BookId,
        };
        return descriptionDto;
    }
}
