using System.Linq.Expressions;
using AutoBackend.Sdk.Data;
using AutoBackend.Sdk.Exceptions;
using AutoBackend.Sdk.Filters;
using AutoBackend.Sdk.Helpers;
using Microsoft.EntityFrameworkCore;

namespace AutoBackend.Sdk.Storage;

internal class GenericFilteredStorage<TEntity, TFilter> :
    GenericStorage<TEntity>,
    IGenericFilteredStorage<TEntity, TFilter>
    where TEntity : class
    where TFilter : class
{
    private readonly GenericDbContext _db;

    public GenericFilteredStorage(GenericDbContext db) : base(db)
    {
        _db = db;
    }

    public Task<TEntity[]> GetAllByFilterAsync(TFilter? filter, CancellationToken cancellationToken)
    {
        return _db.Set<TEntity>().Where(BuildFilterExpression(filter)).ToArrayAsync(cancellationToken);
    }

    public Task<TEntity[]> GetSliceByFilterAsync(
        TFilter? filter,
        int? skipCount, 
        int? takeCount, 
        CancellationToken cancellationToken = default)
    {
        var query = _db.Set<TEntity>().Where(BuildFilterExpression(filter));
        if (skipCount is { } skipCountValue) query = query.Skip(skipCountValue);
        if (takeCount is { } takeCountValue) query = query.Take(takeCountValue);
        return query.ToArrayAsync(cancellationToken);
    }

    public Task<long> CountByFilterAsync(TFilter? filter, CancellationToken cancellationToken)
    {
        return _db.Set<TEntity>().Where(BuildFilterExpression(filter)).LongCountAsync(cancellationToken);
    }

    private Expression<Func<TEntity, bool>> BuildFilterExpression(TFilter? filter)
    {
        var predicate = PredicateBuilder.True<TEntity>();

        if (filter is not null)
            foreach (var filterProperty in filter.GetType().GetProperties())
            {
                var filterValue = filterProperty.GetValue(filter);
                
                if (filterValue is null) continue;
                
                if (filterValue is not IGenericFilter genericFilter)
                    throw new AutoBackendException(
                        $"The filter properties have to be inherited from {nameof(IGenericFilter)}.");

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