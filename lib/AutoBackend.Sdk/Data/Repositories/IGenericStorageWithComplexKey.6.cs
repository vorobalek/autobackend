using AutoBackend.Sdk.Filters;

namespace AutoBackend.Sdk.Data.Repositories;

internal interface IGenericRepositoryWithComplexKey<
    TEntity,
    in TFilter,
    in TKey1,
    in TKey2,
    in TKey3,
    in TKey4,
    in TKey5,
    in TKey6
> : IGenericRepository<TEntity, TFilter> where TEntity : class where TFilter : class, IGenericFilter
{
    Task<TEntity?> GetByComplexKeyAsync(
        TKey1 key1,
        TKey2 key2,
        TKey3 key3,
        TKey4 key4,
        TKey5 key5,
        TKey6 key6,
        CancellationToken cancellationToken);

    Task<TEntity> InsertByComplexKeyAsync(
        TKey1 key1,
        TKey2 key2,
        TKey3 key3,
        TKey4 key4,
        TKey5 key5,
        TKey6 key6,
        TEntity entity,
        CancellationToken cancellationToken);

    Task<TEntity> UpdateByComplexKeyAsync(
        TKey1 key1,
        TKey2 key2,
        TKey3 key3,
        TKey4 key4,
        TKey5 key5,
        TKey6 key6,
        TEntity entity,
        CancellationToken cancellationToken);

    Task DeleteByComplexKeyAsync(
        TKey1 key1,
        TKey2 key2,
        TKey3 key3,
        TKey4 key4,
        TKey5 key5,
        TKey6 key6,
        CancellationToken cancellationToken);
}