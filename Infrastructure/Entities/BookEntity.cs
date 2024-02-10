using Infrastructure.Dtos;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Entities;

public class BookEntity
{
    [Key]
    // id som pk gör att det går att ändra isbn utan att de andra tabellerna påverkas, ny utgåva = nytt isbn?
    public int Id { get; set; }
    [Required]
    [Column(TypeName = "nvarchar (20)")]
    public string ISBN { get; set; } = null!;
    // sätt som string eftersom ett isbn kan innehålla bokstav i slutet
    [Required]
    [Column(TypeName = "nvarchar (200)")]
    public string Title { get; set; } = null!;
    [Required]
    [Column(TypeName = "nvarchar (100)")]
    public string Author { get; set; } = null!;
    [Required]
    [Column(TypeName = "money")]
    public int Price { get; set; }
    public int CategoryId { get; set; }
    public CategoryEntity Category { get; set; } = null!;

    // navigering skapar relationen MÅNGA till en. En bok med unikt isbn nummer kommer ha en beskrivning 
    // men det kan finnas många beskrivningar till ett isbn/ en bok. ex: kan finnas på flera olika språk
    public ICollection<DescriptionEntity> Description { get; set; } = new HashSet<DescriptionEntity>();

    public ICollection<BookPublisherEntity> BookPublisher { get; set; } = new HashSet<BookPublisherEntity>();   
    
    //mappa här istället , lättare att ändra i senare
    public static implicit operator BookEntity(BookDto bookdto)
    {
        return new BookEntity
        {
          
            ISBN = bookdto.ISBN,
            Title = bookdto.Title,
            Author = bookdto.Author,
            Price = bookdto.Price,
            CategoryId = bookdto.CategoryId,
        };
    }
}
