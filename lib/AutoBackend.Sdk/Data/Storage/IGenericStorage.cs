using System.Linq.Expressions;
using AutoBackend.Sdk.Exceptions;
using AutoBackend.Sdk.Filters;
using AutoBackend.Sdk.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace AutoBackend.Sdk.Data.Storage;

internal interface IGenericStorage<TEntity, in TFilter>
    where TEntity : class
    where TFilter : class, IGenericFilter
{
    ValueTask<TEntity?> FindAsync(object?[]? keyValues, CancellationToken cancellationToken = default);
    ValueTask<EntityEntry<TEntity>> AddAsync(TEntity entity, CancellationToken cancellationToken = default);
    EntityEntry<TEntity> Update(TEntity entity);
    EntityEntry<TEntity> Remove(TEntity entity);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    IQueryable<TEntity> GetQuery(TFilter? filter);
    EntityEntry<TEntity> Entry(TEntity entity);
}

internal class GenericStorage<TEntity, TFilter> : IGenericStorage<TEntity, TFilter>
    where TEntity : class
    where TFilter : class, IGenericFilter
{
    private readonly GenericDbContext _db;

    public GenericStorage(GenericDbContext db)
    {
        _db = db;
    }

    private DbSet<TEntity> Set => _db.Set<TEntity>();

    public ValueTask<TEntity?> FindAsync(object?[]? keyValues, CancellationToken cancellationToken = default)
    {
        return Set.FindAsync(keyValues, cancellationToken);
    }

    public ValueTask<EntityEntry<TEntity>> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        return Set.AddAsync(entity, cancellationToken);
    }

    public EntityEntry<TEntity> Update(TEntity entity)
    {
        return Set.Update(entity);
    }

    public EntityEntry<TEntity> Remove(TEntity entity)
    {
        return Set.Remove(entity);
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _db.SaveChangesAsync(cancellationToken);
    }

    public IQueryable<TEntity> GetQuery(TFilter? filter)
    {
        return Set.Where(BuildFilterExpression(filter));
    }

    public EntityEntry<TEntity> Entry(TEntity entity)
    {
        return _db.Entry(entity);
    }

    private Expression<Func<TEntity, bool>> BuildFilterExpression(TFilter? filter)
    {
        var predicate = PredicateBuilder.True<TEntity>();

        if (filter is not null)
            foreach (var filterProperty in filter.GetType().GetProperties())
            {
                var filterValue = filterProperty.GetValue(filter);

                if (filterValue is null) continue;

                if (filterValue is not IGenericPropertyFilter genericFilter)
                    throw new AutoBackendException(
                        $"The filter properties have to be inherited from {nameof(IGenericPropertyFilter)}.");

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
}