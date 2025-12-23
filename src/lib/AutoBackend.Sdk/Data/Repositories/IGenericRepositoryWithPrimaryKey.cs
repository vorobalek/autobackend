using AutoBackend.Sdk.Filters;

namespace AutoBackend.Sdk.Data.Repositories;

internal interface IGenericRepositoryWithPrimaryKey<
    TEntity,
    in TFilter,
    in TKey
> : IGenericRepository<
    TEntity,
    TFilter
>
    where TEntity : class, new()
    where TFilter : class, IGenericFilter
    where TKey : notnull
{
    Task<TEntity?> GetByPrimaryKeyAsync(TKey key, CancellationToken cancellationToken);
    Task<TEntity> CreateByPrimaryKeyAsync(TKey key, TEntity entity, CancellationToken cancellationToken);
    Task<TEntity> UpdateByPrimaryKeyAsync(TKey key, TEntity entity, CancellationToken cancellationToken);
    Task DeleteByPrimaryKeyAsync(TKey key, CancellationToken cancellationToken);
}