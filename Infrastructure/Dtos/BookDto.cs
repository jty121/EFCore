using Infrastructure.Entities;

namespace Infrastructure.Dtos;

public class BookDto
{
    public int Id { get; set; } //här ska inte vara något id, det här är bara en modell för en bok...
    public string ISBN { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string Author { get; set; } = null!;
    public int Price { get; set; }
    public int CategoryId { get; set; }
    public string CategoryName { get; set; } = null!;
    public string PublisherName { get; set; } = null!;
    public List<DescriptionDto> Descriptions { get; set; } = [];

    public static implicit operator BookDto(BookEntity book)
    {
        var bookDto = new BookDto
        {
            Id = book.Id,
            ISBN = book.ISBN,
            Title = book.Title,
            Author = book.Author,
            Price = book.Price,
            CategoryId = book.CategoryId,
        };
        return bookDto; 
    }
}
