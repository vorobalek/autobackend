namespace AutoBackend.Sdk.Storage.Generic;

internal interface IGenericStorageWithComplexKey<
    TEntity,
    in TKey1,
    in TKey2,
    in TKey3
> : IGenericStorage<TEntity> where TEntity : class
{
    Task<TEntity?> GetByComplexKeyAsync(
        TKey1 key1,
        TKey2 key2,
        TKey3 key3,
        CancellationToken cancellationToken);

    Task<TEntity> InsertByComplexKeyAsync(
        TKey1 key1,
        TKey2 key2,
        TKey3 key3,
        TEntity entity,
        CancellationToken cancellationToken);

    Task<TEntity> UpdateByComplexKeyAsync(
        TKey1 key1,
        TKey2 key2,
        TKey3 key3,
        TEntity entity,
        CancellationToken cancellationToken);

    Task DeleteByComplexKeyAsync(
        TKey1 key1,
        TKey2 key2,
        TKey3 key3,
        CancellationToken cancellationToken);
}