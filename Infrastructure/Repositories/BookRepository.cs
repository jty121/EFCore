using Infrastructure.Contexts;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Infrastructure.Repositories;

//bookrepository ärver från baserepository, behöver bara ändra på de metoderna som du vill göra något med. 
public class BookRepository(DataContext context) : BaseRepository<BookEntity>(context)
{
    private readonly DataContext _context = context;

    public override async Task<IEnumerable<BookEntity>> GetAllAsync()
    {
        try
        {
            var entityExist = await _context.Books
                .Include(i => i.Description)
                .Include(i => i.BookPublisher).ThenInclude(i => i.Manufacture)
                .ToListAsync();
            if (entityExist != null)
            {
                return entityExist;
            }

        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }
        return null!;
    }

    public override async Task<BookEntity> GetAsync(Expression<Func<BookEntity, bool>> predicate)
    {
        try
        {
            var entityExist = await _context.Books.Include(i => i.Category)
                .Include(i => i.Description)
                .Include(i => i.BookPublisher).ThenInclude(i => i.Manufacture)
                .FirstOrDefaultAsync(predicate);
            if (entityExist != null)
            {
                return entityExist;
            }

        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }
        return null!;
    }

    public override async Task<BookEntity> UpdateAsync(Expression<Func<BookEntity, bool>> predicate)
    {
        try
        {
            var entityExist = await _context.Books
                .Include(i => i.Description)
                .Include(i => i.Category)
                .Include(i => i.BookPublisher).ThenInclude(i => i.Manufacture)
                .FirstOrDefaultAsync(predicate);
            if (entityExist != null)
            {
                return entityExist;
            }
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }
        return null!;
    }
}
