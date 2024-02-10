using Infrastructure.Contexts;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Infrastructure.Repositories;

public class DescriptionRepository(DataContext context) : BaseRepository<DescriptionEntity>(context)
{
    private readonly DataContext _context = context;

    public override async Task<IEnumerable<DescriptionEntity>> GetAllAsync()
    {
        try
        {
            var entityExist = await _context.Descriptions
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

    public override async Task<DescriptionEntity> GetAsync(Expression<Func<DescriptionEntity, bool>> predicate)
    {
        try
        {
            var entityExist = await _context.Descriptions
                .Include(i => i.Book)
                .FirstOrDefaultAsync(predicate);
            if (entityExist != null)
            {
                return entityExist;
            }
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }
        return null!;
    }

    public override async Task<DescriptionEntity> UpdateAsync(Expression<Func<DescriptionEntity, bool>> predicate) //ändrat här håller på med update metoden
    {
        try
        {
            var entityExist = await _context.Descriptions
                .Include(i => i.Book)
                .FirstOrDefaultAsync(predicate);
            if (entityExist != null)
            {
                _context.Entry(entityExist).CurrentValues.SetValues(predicate);
                await _context.SaveChangesAsync();
                return entityExist;
            }
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }
        return null!;
    }
}
