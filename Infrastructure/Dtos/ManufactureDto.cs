using Infrastructure.Entities;

namespace Infrastructure.Dtos;

public class ManufactureDto
{
    public int Id { get; set; }
    public string PublisherName { get; set; } = null!;

    public static implicit operator ManufactureDto(ManufactureEntity publisher)
    {
        var publisherDto = new ManufactureDto
        {
            Id = publisher.Id,
            PublisherName = publisher.PublisherName
        };
        return publisherDto;
    }
}
