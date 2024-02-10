using Infrastructure.Contexts;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Infrastructure.Repositories;

//Kör override på det som du specifikt vill ändra
public class ManufactureRepository(DataContext context) : BaseRepository<ManufactureEntity>(context)
{
    private readonly DataContext _context = context;

    public override async Task<IEnumerable<ManufactureEntity>> GetAllAsync()
    {
        try
        {
            var entityExist = await _context.Manufactures
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

    public override async Task<ManufactureEntity> GetAsync(Expression<Func<ManufactureEntity, bool>> predicate)
    {
        try
        {
            var entityExist = await _context.Manufactures
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

    public override async Task<ManufactureEntity> GetAsync(ManufactureEntity entity)
    {
        try
        {
            var entityExist = await _context.Manufactures
                .Include(i => i.BookPublisher).ThenInclude(i => i.Manufacture)
                .FirstOrDefaultAsync();
            if (entityExist != null)
            {
                return entityExist;
            }
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }
        return null!;
    }
}
