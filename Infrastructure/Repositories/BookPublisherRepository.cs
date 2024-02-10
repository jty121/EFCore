using Infrastructure.Contexts;
using Infrastructure.Entities;

namespace Infrastructure.Repositories;

public class BookPublisherRepository(DataContext context) : BaseRepository<BookPublisherEntity>(context)
{
    private readonly DataContext _context = context;
}
