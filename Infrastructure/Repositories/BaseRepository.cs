using Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Infrastructure.Repositories;

public abstract class BaseRepository<TEntity> where TEntity : class
{
    private readonly DataContext _context;

    protected BaseRepository(DataContext context)
    {
        _context = context;
    }

    // generisk klass, här skapas generella metoder för att utföra CRUD mot databasen som mina andra repositories ärver från
    // async anrop gör applikationen mer flexibel. parallel körning = påverkar inte huvudapplikationen, renderingen sker på en egen tråd
 
    public virtual async Task<TEntity> CreateAsync(TEntity entity)
    {
        try
        {
            _context.Set<TEntity>().Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }
        return null!;
    }

    public virtual async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate)
    {
        try
        {
           var entityExist = await _context.Set<TEntity>().FirstOrDefaultAsync(predicate);
            if (entityExist != null)
            {
                return entityExist;
            }
            
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }
        return null!;
    }

    public virtual async Task<TEntity> GetAsync(TEntity entity)
    {
        try
        {
            var entityExist = await _context.Set<TEntity>().FirstOrDefaultAsync();
            if (entityExist != null)
            {
                return entityExist;
            }

        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }
        return null!;
    }

    public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        try
        {
            var entityExist = await _context.Set<TEntity>().ToListAsync();
            if (entityExist != null)
            {
                return entityExist;
            }

        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }
        return null!;
    }

    public virtual async Task<TEntity> UpdateAsync(Expression<Func<TEntity, bool>> predicate)
    {
        try
        {
            var entityExist = await _context.Set<TEntity>().FirstOrDefaultAsync(predicate);
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

    public virtual async Task<TEntity> UpdateOneAsync(TEntity entity)
    {
        try
        {
            // sätter du entitystate.modified så talar du om för entityframework att ändringar har gjorts
            // och behöver sparas till databasen. Effektivt om man ska uppdatera hela entiteter? 
            // ska man bara ändra något litet i en entitet kanske det är bättre med den andra
                _context.Entry(entity).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return entity;

        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }
        return null!;
    }
    public virtual async Task<bool> DeleteAsync(Expression<Func<TEntity, bool>> predicate)
    {
        try
        {
            var entityExist = await _context.Set<TEntity>().FirstOrDefaultAsync(predicate);
            if (entityExist != null)
            {
                _context.Set<TEntity>().Remove(entityExist);
                await _context.SaveChangesAsync();
                return true;
            }
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }
        return false!;
    }

    public virtual async Task<bool> ExistingAsync(Expression<Func<TEntity, bool>> predicate)
    {
        try
        {
            var entityExist = await _context.Set<TEntity>().AnyAsync(predicate);
            return entityExist;
           
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }
        return false!;
    }
}
