using Infrastructure.Contexts;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Infrastructure.Repositories;

public class CategoryRepository(DataContext context) : BaseRepository<CategoryEntity>(context)
{
    private readonly DataContext _context = context;

    public override async Task<IEnumerable<CategoryEntity>> GetAllAsync()
    {
        try
        {
            var entityExist = await _context.Categories
                .Include(i => i.Book)
                .ToListAsync();
            if (entityExist != null)
            {
                return entityExist;
            }
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }
        return null!;
    }
    public override async Task<CategoryEntity> GetAsync(Expression<Func<CategoryEntity, bool>> predicate)
    {
        try
        {
            var entityExist = await _context.Categories
                .Include(i => i.Book)
                .FirstOrDefaultAsync();
            if (entityExist != null)
            {
                return entityExist;
            }
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }
        return null!;
    }

    public override async Task<CategoryEntity> GetAsync(CategoryEntity entity)
    {
        try
        {
            var entityExist = await _context.Categories
                .Include(i => i.Book)
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
