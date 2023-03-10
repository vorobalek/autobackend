using System.Linq.Expressions;
using AutoBackend.Sdk.Data;
using AutoBackend.Sdk.Exceptions.Api;
using AutoBackend.Sdk.Filters.Generic;
using AutoBackend.Sdk.Helpers;
using Microsoft.EntityFrameworkCore;

namespace AutoBackend.Sdk.Storage.Generic;

internal class GenericStorage<TEntity> : IGenericStorage<TEntity> where TEntity : class
{
    private readonly AutoBackendDbContext _db;

    // ReSharper disable once MemberCanBeProtected.Global
    public GenericStorage(AutoBackendDbContext db)
    {
        _db = db;
    }

    public Task<TEntity[]> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return _db.Set<TEntity>().ToArrayAsync(cancellationToken);
    }

    public Task<TEntity[]> GetByFilterAsync<TFilter>(TFilter? filter, CancellationToken cancellationToken)
        where TFilter : class
    {
        return _db.Set<TEntity>().Where(BuildFilterExpression(filter)).ToArrayAsync(cancellationToken);
    }

    public Task<int> CountByFilterAsync<TFilter>(TFilter? filter, CancellationToken cancellationToken)
        where TFilter : class
    {
        return _db.Set<TEntity>().Where(BuildFilterExpression(filter)).CountAsync(cancellationToken);
    }

    private Expression<Func<TEntity, bool>> BuildFilterExpression<TFilter>(TFilter filter)
    {
        var predicate = PredicateBuilder.True<TEntity>();

        if (filter is not null)
            foreach (var filterProperty in filter.GetType().GetProperties())
            {
                var filterValue = filterProperty.GetValue(filter);
                if (filterValue is not IGenericFilter genericFilter) continue;

                if (genericFilter.Equal is not null)
                    predicate = predicate.And(genericFilter.EqualExpr<TEntity>(filterProperty.Name));

                if (genericFilter.NotEqual is not null)
                    predicate = predicate.And(genericFilter.NotEqualExpr<TEntity>(filterProperty.Name));

                if (genericFilter.IsNull is not null)
                    predicate = predicate.And(genericFilter.IsNullExpr<TEntity>(filterProperty.Name));

                if (genericFilter.GreaterThan is not null)
                    predicate = predicate.And(genericFilter.GreaterThanExpr<TEntity>(filterProperty.Name));

                if (genericFilter.GreaterThanOrEqual is not null)
                    predicate = predicate.And(genericFilter.GreaterThanOrEqualExpr<TEntity>(filterProperty.Name));

                if (genericFilter.LessThan is not null)
                    predicate = predicate.And(genericFilter.LessThanExpr<TEntity>(filterProperty.Name));

                if (genericFilter.LessThanOrEqual is not null)
                    predicate = predicate.And(genericFilter.LessThanOrEqualExpr<TEntity>(filterProperty.Name));

                if (genericFilter.In is not null)
                    predicate = predicate.And(genericFilter.InExpr<TEntity>(filterProperty.Name));
            }

        return predicate;
    }

    protected async Task<TEntity?> GetByKeyInternalAsync(
        CancellationToken cancellationToken,
        params object?[]? keys)
    {
        return await _db.Set<TEntity>().FindAsync(keys, cancellationToken);
    }

    protected async Task<TEntity> InsertInternalAsync(
        TEntity entity,
        CancellationToken cancellationToken,
        params object?[]? keys)
    {
        var current = await _db.Set<TEntity>().FindAsync(keys);
        if (current is not null)
            throw new BadRequestApiException(
                $"Entity with same key already exists ({string.Join(',', keys?.Select(key => key?.ToString()) ?? Array.Empty<string?>())})");

        await _db.Set<TEntity>().AddAsync(entity, cancellationToken);
        await _db.SaveChangesAsync(cancellationToken);
        return entity;
    }

    protected async Task<TEntity> UpdateInternalAsync(
        TEntity entity,
        CancellationToken cancellationToken,
        params object?[]? keys)
    {
        var current = await _db.Set<TEntity>().FindAsync(keys);
        if (current is null)
            throw new BadRequestApiException(
                $"Entity with given key does not exists ({string.Join(',', keys?.Select(key => key?.ToString()) ?? Array.Empty<string?>())})");

        foreach (var entryProperty in _db.Entry(current).Properties)
        {
            var entityProperty = typeof(TEntity).GetProperty(entryProperty.Metadata.Name);
            if (entityProperty is null)
                throw new NotFoundApiException(
                    $"Unable to find property with name {entryProperty.Metadata.Name} in object {typeof(TEntity).Name}");
            var newValue = entityProperty.GetValue(entity);
            entityProperty.SetValue(current, newValue);
        }

        _db.Set<TEntity>().Update(current);
        await _db.SaveChangesAsync(cancellationToken);
        return entity;
    }

    protected async Task DeleteInternalAsync(
        CancellationToken cancellationToken,
        params object?[]? keys)
    {
        var current = await _db.Set<TEntity>().FindAsync(keys);
        if (current is null)
            throw new NotFoundApiException(
                $"Entity with given key does not exists ({string.Join(',', keys?.Select(key => key?.ToString()) ?? Array.Empty<string?>())})");

        _db.Set<TEntity>().Remove(current);
        await _db.SaveChangesAsync(cancellationToken);
    }
}