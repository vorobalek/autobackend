namespace AutoBackend.Sdk.Storage;

internal interface IGenericStorageWithComplexKey<
    TEntity,
    in TKey1,
    in TKey2,
    in TKey3,
    in TKey4
> : IGenericStorage<TEntity> where TEntity : class
{
    Task<TEntity?> GetByComplexKeyAsync(
        TKey1 key1,
        TKey2 key2,
        TKey3 key3,
        TKey4 key4,
        CancellationToken cancellationToken);

    Task<TEntity> InsertByComplexKeyAsync(
        TKey1 key1,
        TKey2 key2,
        TKey3 key3,
        TKey4 key4,
        TEntity entity,
        CancellationToken cancellationToken);

    Task<TEntity> UpdateByComplexKeyAsync(
        TKey1 key1,
        TKey2 key2,
        TKey3 key3,
        TKey4 key4,
        TEntity entity,
        CancellationToken cancellationToken);

    Task DeleteByComplexKeyAsync(
        TKey1 key1,
        TKey2 key2,
        TKey3 key3,
        TKey4 key4,
        CancellationToken cancellationToken);
}