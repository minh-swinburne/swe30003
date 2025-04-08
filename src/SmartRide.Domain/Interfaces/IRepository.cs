using SmartRide.Domain.Entities.Base;
using System.Linq.Expressions;

namespace SmartRide.Domain.Interfaces;

public interface IRepository<T> where T : BaseEntity
{
    IQueryable<T> Query(CancellationToken cancellationToken = default);
    Task<int> CountAsync(CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);
    Task<T> CreateAsync(T entity, CancellationToken cancellationToken = default);
    Task<T> UpdateAsync(T entity, CancellationToken cancellationToken = default);
    Task<T> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<T?> GetByIdAsync(Guid id, List<string>? includes = null, CancellationToken cancellationToken = default);
    Task<List<T>> GetAllAsync(List<string>? includes = null, CancellationToken cancellationToken = default);
    Task<List<T>> GetWithFilterAsync<TDto>(
        Expression<Func<T, bool>>? filter,
        Expression<Func<T, TDto>>? select = null,
        Expression<Func<T, object>>? orderBy = null,
        bool ascending = true,
        int skip = 0,
        int limit = 0,
        List<string>? includes = null,
        CancellationToken cancellationToken = default
        );
}
