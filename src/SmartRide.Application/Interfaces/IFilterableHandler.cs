using SmartRide.Domain.Entities;
using System.Linq.Expressions;

namespace SmartRide.Application.Interfaces;

public interface IFilterableHandler<TQuery, TEntity>
    where TQuery : class 
    where TEntity : BaseEntity
{
    Expression<Func<TEntity, bool>>? BuildFilter(TQuery query);
}
