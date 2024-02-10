using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
namespace Infrastructure.Contexts;

public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
    public virtual DbSet<BookEntity> Books { get; set; }
    public virtual DbSet<BookPublisherEntity> BookPublishers { get; set; }
    public virtual DbSet<CategoryEntity> Categories { get; set; }
    public virtual DbSet<DescriptionEntity> Descriptions { get; set; }
    public virtual DbSet<ManufactureEntity> Manufactures { get; set; } 

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BookEntity>()
            .HasIndex(x => x.ISBN) 
            .IsUnique();
        //varje ISBN nummer ska vara unikt

        modelBuilder.Entity<BookPublisherEntity>()
            .HasKey(x => new { x.BookId, x.ManufactureId });
        //kopplingstabellens sammansatta nycklar
    }
}
