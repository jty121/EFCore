namespace Infrastructure.Dtos;

public class AddDescriptionToBookDto
{
    public int Id { get; set; }
    public string ISBN { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string Author { get; set; } = null!;
    public int Price { get; set; }
    public string Description { get; set; } = null!;
    public string Language { get; set; } = null!;
    public string Narrator { get; set; } = null!;
    public string? Illustrator { get; set; }
    public int BookId { get; set; }
}
