using Infrastructure.Dtos;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Entities;

public class ManufactureEntity
{
    [Key]
    public int Id { get; set; }
    [Required]
    [Column(TypeName = "nvarchar (200)")]
    public string PublisherName { get; set; } = null!;

    public ICollection<BookPublisherEntity> BookPublisher { get; set; } = new HashSet<BookPublisherEntity>();

    public static implicit operator ManufactureEntity(ManufactureDto publisher)
    {
        return new ManufactureEntity
        {
            Id = publisher.Id,
            PublisherName = publisher.PublisherName
        };
    }
}
