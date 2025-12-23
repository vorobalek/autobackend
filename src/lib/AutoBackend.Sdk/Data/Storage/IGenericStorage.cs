using AutoBackend.Sdk.Filters;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace AutoBackend.Sdk.Data.Storage;

internal interface IGenericStorage<TEntity, in TFilter>
    where TEntity : class, new()
    where TFilter : class, IGenericFilter
{
    ValueTask<TEntity?> FindAsync(object[] keyValues, CancellationToken cancellationToken);
    ValueTask<EntityEntry<TEntity>> AddAsync(TEntity entity, CancellationToken cancellationToken);
    EntityEntry<TEntity> Update(TEntity entity);
    EntityEntry<TEntity> Remove(TEntity entity);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    IQueryable<TEntity> GetQuery(TFilter? filter);
    EntityEntry<TEntity> Entry(TEntity entity);
}