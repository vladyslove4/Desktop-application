using System.Linq.Expressions;

namespace DesktopApp.Domain.Interfaces;

public interface IBaseRepository<T>
{
    Task<T> CreateAsync(T entity, CancellationToken cancellationToken);

    Task<T> GetByIdAsync(int id, CancellationToken cancellationToken);

    Task<IReadOnlyList<T>> GetAllAsync(CancellationToken cancellationToken);

    Task<IReadOnlyList<T>> FindAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken);

    Task<T> UpdateAsync(T entity, CancellationToken cancellationToken);

    Task<T> DeleteAsync(T entity, CancellationToken cancellationToken);
}