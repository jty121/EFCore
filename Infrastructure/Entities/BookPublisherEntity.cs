namespace Infrastructure.Entities;

public class BookPublisherEntity
{
    // kopplingstabell för MÅNGA - MÅNGA relation mellan böcker och utgivare
    public int BookId { get; set; }
    public BookEntity Book { get; set; } = null!;

    public int ManufactureId { get; set; }
    public ManufactureEntity Manufacture { get; set; } = null!;
}