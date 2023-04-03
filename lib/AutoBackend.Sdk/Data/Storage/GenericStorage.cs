using System.Linq.Expressions;
using AutoBackend.Sdk.Exceptions.Reflection;
using AutoBackend.Sdk.Filters;
using AutoBackend.Sdk.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace AutoBackend.Sdk.Data.Storage;

internal sealed class GenericStorage<TEntity, TFilter> : IGenericStorage<TEntity, TFilter>
    where TEntity : class
    where TFilter : class, IGenericFilter
{
    private readonly GenericDbContext _db;

    public GenericStorage(GenericDbContext db)
    {
        _db = db;
    }

    private DbSet<TEntity> Set => _db.Set<TEntity>();

    public ValueTask<TEntity?> FindAsync(object[] keyValues, CancellationToken cancellationToken)
    {
        return Set.FindAsync(keyValues, cancellationToken);
    }

    public ValueTask<EntityEntry<TEntity>> AddAsync(TEntity entity, CancellationToken cancellationToken)
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

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken)
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

        if (filter is null) return predicate;

        foreach (var filterProperty in filter.GetType().GetProperties())
        {
            var filterPropertyValue = filterProperty.GetValue(filter);

            if (filterPropertyValue is null) continue;

            if (filterPropertyValue is not IGenericPropertyFilter genericPropertyFilter)
                throw new InheritanceReflectionException(
                    string.Format(
                        Constants.TheFilterPropertiesHaveToBeInheritedFrom,
                        nameof(IGenericPropertyFilter)));

            if (genericPropertyFilter.Equal is not null)
                predicate = predicate.And(genericPropertyFilter.EqualExpr<TEntity>(filterProperty.Name));

            if (genericPropertyFilter.NotEqual is not null)
                predicate = predicate.And(genericPropertyFilter.NotEqualExpr<TEntity>(filterProperty.Name));

            if (genericPropertyFilter.IsNull is not null)
                predicate = predicate.And(genericPropertyFilter.IsNullExpr<TEntity>(filterProperty.Name));

            if (genericPropertyFilter.GreaterThan is not null)
                predicate = predicate.And(genericPropertyFilter.GreaterThanExpr<TEntity>(filterProperty.Name));

            if (genericPropertyFilter.GreaterThanOrEqual is not null)
                predicate = predicate.And(genericPropertyFilter.GreaterThanOrEqualExpr<TEntity>(filterProperty.Name));

            if (genericPropertyFilter.LessThan is not null)
                predicate = predicate.And(genericPropertyFilter.LessThanExpr<TEntity>(filterProperty.Name));

            if (genericPropertyFilter.LessThanOrEqual is not null)
                predicate = predicate.And(genericPropertyFilter.LessThanOrEqualExpr<TEntity>(filterProperty.Name));

            if (genericPropertyFilter.In is not null)
                predicate = predicate.And(genericPropertyFilter.InExpr<TEntity>(filterProperty.Name));
        }

        return predicate;
    }
}