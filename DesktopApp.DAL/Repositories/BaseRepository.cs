using DesktopApp.Domain.Exceptions;
using DesktopApp.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DesktopApp.DAL.Repositories;

public class BaseRepository<T> : IBaseRepository<T> where T : class, IEntity
{
    private readonly ApplicationDbContext _dbContext;

    public BaseRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<T> CreateAsync(T entity, CancellationToken cancellationToken)
    {
        await _dbContext.AddAsync(entity, cancellationToken);
        var affectedRecord = await _dbContext.SaveChangesAsync(cancellationToken);
        if (affectedRecord <= 0)
        {
            throw new CannotCreateEntityException($"Can not create entity");
        }

        return entity;
    }

    public async Task<T> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var entity = await _dbContext.Set<T>().FindAsync(id, cancellationToken);
        if (entity is null)
        {
           throw new CannotFindEntityException($"Entity with {id} not found");
        }

        return entity;
    }

    public async Task<IReadOnlyList<T>> GetAllAsync(CancellationToken cancellationToken)
    {
        var result = await _dbContext.Set<T>().ToListAsync(cancellationToken);
        return result.AsReadOnly();
    }

    public async Task<IReadOnlyList<T>> FindAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken)
    {
        var result = await _dbContext.Set<T>().Where(predicate).ToListAsync(cancellationToken);
        return result.AsReadOnly();
    }

    public async Task<T> UpdateAsync(T entity, CancellationToken cancellationToken)
    {
        var existingEntity = _dbContext.Set<T>().Local.FirstOrDefault(e => e.Id == entity.Id);

        if (existingEntity != null)
        {
            _dbContext.Entry(existingEntity).State = EntityState.Detached;
        }

        _dbContext.Entry(entity).State = EntityState.Modified;
        
        var affectedRecord = await _dbContext.SaveChangesAsync(cancellationToken);
        if (affectedRecord <= 0)
        {
            throw new CannotUpdateEntityException($"Can not update entity");
        }

        return entity;
    }

    public async Task<T> DeleteAsync(T entity, CancellationToken cancellationToken)
    {
        _dbContext.Set<T>().Remove(entity);
        var affectedRecord = await _dbContext.SaveChangesAsync(cancellationToken);
        if (affectedRecord <= 0)
        {
            throw new CannotDeleteEntityException($"Can not delete entity");
        }

        return entity;
    }
}