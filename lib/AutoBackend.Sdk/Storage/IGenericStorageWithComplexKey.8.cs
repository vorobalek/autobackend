namespace AutoBackend.Sdk.Storage;

internal interface IGenericStorageWithComplexKey<
    TEntity,
    in TKey1,
    in TKey2,
    in TKey3,
    in TKey4,
    in TKey5,
    in TKey6,
    in TKey7,
    in TKey8
> : IGenericStorage<TEntity> where TEntity : class
{
    Task<TEntity?> GetByComplexKeyAsync(
        TKey1 key1,
        TKey2 key2,
        TKey3 key3,
        TKey4 key4,
        TKey5 key5,
        TKey6 key6,
        TKey7 key7,
        TKey8 key8,
        CancellationToken cancellationToken);

    Task<TEntity> InsertByComplexKeyAsync(
        TKey1 key1,
        TKey2 key2,
        TKey3 key3,
        TKey4 key4,
        TKey5 key5,
        TKey6 key6,
        TKey7 key7,
        TKey8 key8,
        TEntity entity,
        CancellationToken cancellationToken);

    Task<TEntity> UpdateByComplexKeyAsync(
        TKey1 key1,
        TKey2 key2,
        TKey3 key3,
        TKey4 key4,
        TKey5 key5,
        TKey6 key6,
        TKey7 key7,
        TKey8 key8,
        TEntity entity,
        CancellationToken cancellationToken);

    Task DeleteByComplexKeyAsync(
        TKey1 key1,
        TKey2 key2,
        TKey3 key3,
        TKey4 key4,
        TKey5 key5,
        TKey6 key6,
        TKey7 key7,
        TKey8 key8,
        CancellationToken cancellationToken);
}