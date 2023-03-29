using AutoBackend.Sdk.Filters;

namespace AutoBackend.Sdk.Data.Repositories;

internal interface IGenericRepositoryWithComplexKey<
    TEntity,
    in TFilter,
    in TKey1,
    in TKey2
> : IGenericRepository<TEntity, TFilter> where TEntity : class where TFilter : class, IGenericFilter
{
    Task<TEntity?> GetByComplexKeyAsync(TKey1 key1,
        TKey2 key2,
        CancellationToken cancellationToken);

    Task<TEntity> InsertByComplexKeyAsync(
        TKey1 key1,
        TKey2 key2,
        TEntity entity,
        CancellationToken cancellationToken);

    Task<TEntity> UpdateByComplexKeyAsync(
        TKey1 key1,
        TKey2 key2,
        TEntity entity,
        CancellationToken cancellationToken);

    Task DeleteByComplexKeyAsync(
        TKey1 key1,
        TKey2 key2,
        CancellationToken cancellationToken);
}