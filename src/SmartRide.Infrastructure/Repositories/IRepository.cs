using SmartRide.Domain.Entities.Base;
using System.Linq.Expressions;

namespace SmartRide.Infrastructure.Repositories;

public interface IRepository<T> where T : Entity
{
    IQueryable<T> Query(CancellationToken cancellationToken = default);
    Task<int> CountAsync(CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(string id, CancellationToken cancellationToken = default);
    Task<T> CreateAsync(T entity, CancellationToken cancellationToken = default);
    Task<T> UpdateAsync(T entity, CancellationToken cancellationToken = default);
    Task<T> DeleteAsync(string id, CancellationToken cancellationToken = default);
    Task<T?> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<T>> GetByFilterAsync<TDto>(
        Expression<Func<T, bool>> filter,
        Expression<Func<T, TDto>>? select = null,
        Expression<Func<T, object>>? orderBy = null,
        bool ascending = true,
        int skip = 0,
        int limit = 0,
        CancellationToken cancellationToken = default
        );
}
