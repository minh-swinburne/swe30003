﻿using Microsoft.EntityFrameworkCore;
using SmartRide.Common.Extensions;
using SmartRide.Domain.Entities;
using SmartRide.Domain.Interfaces;
using System.Linq.Expressions;

namespace SmartRide.Infrastructure.Persistence;

public class Repository<T>(SmartRideDbContext dbContext) : IRepository<T> where T : BaseEntity
{
    private readonly SmartRideDbContext _dbContext = dbContext;
    private readonly DbSet<T> _dbSet = dbContext.Set<T>();

    public IQueryable<T> Query(CancellationToken cancellationToken = default)
    {
        return _dbSet.AsQueryable();
    }

    public async Task<int> CountAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet.CountAsync(cancellationToken);
    }

    public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbSet.AnyAsync(e => e.Id == id, cancellationToken);
    }

    public async Task<T> CreateAsync(T entity, CancellationToken cancellationToken = default)
    {
        await _dbSet.AddAsync(entity, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task<T> UpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
        _dbSet.Update(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task<T> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await _dbSet.FindAsync([id], cancellationToken: cancellationToken)
            ?? throw new KeyNotFoundException($"Entity of type {typeof(T)} with id {id} not found.");

        _dbSet.Remove(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbSet.FindAsync([id], cancellationToken: cancellationToken);
    }

    public async Task<List<T>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet.ToListAsync(cancellationToken);
    }

    public async Task<List<T>> GetWithFilterAsync<TDto>(
        Expression<Func<T, bool>>? filter,
        Expression<Func<T, TDto>>? select = null,
        Expression<Func<T, object>>? orderBy = null,
        bool ascending = true,
        int skip = 0,
        int limit = 0,
        CancellationToken cancellationToken = default
        )
    {
        var query = _dbSet.AsQueryable();

        if (filter != null)
            query = query.Where(filter);

        if (select != null)
            query = (IQueryable<T>)query.Select(select);

        if (orderBy != null)
            query = ascending ? query.OrderBy(orderBy) : query.OrderByDescending(orderBy);

        if (skip != 0 && limit != 0)
            query = query.Paginate(skip, limit);
        else if (skip != 0)
            query = query.Skip(skip);
        else if (limit != 0)
            query = query.Take(limit);

        return await query.ToListAsync(cancellationToken);
    }
}
