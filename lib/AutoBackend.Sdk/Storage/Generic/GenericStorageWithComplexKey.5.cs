using AutoBackend.Sdk.Data;

namespace AutoBackend.Sdk.Storage.Generic;

internal class GenericStorageWithComplexKey<
    TEntity,
    TKey1,
    TKey2,
    TKey3,
    TKey4,
    TKey5
> : GenericStorage<
    TEntity
>, IGenericStorageWithComplexKey<
    TEntity,
    TKey1,
    TKey2,
    TKey3,
    TKey4,
    TKey5
> where TEntity : class
{
    public GenericStorageWithComplexKey(AutoBackendDbContext db) : base(db)
    {
    }

    public Task<TEntity?> GetByComplexKeyAsync(
        TKey1 key1,
        TKey2 key2,
        TKey3 key3,
        TKey4 key4,
        TKey5 key5,
        CancellationToken cancellationToken)
    {
        return GetByKeyInternalAsync(
            cancellationToken,
            key1,
            key2,
            key3,
            key4,
            key5);
    }

    public Task<TEntity> InsertByComplexKeyAsync(
        TKey1 key1,
        TKey2 key2,
        TKey3 key3,
        TKey4 key4,
        TKey5 key5,
        TEntity entity,
        CancellationToken cancellationToken)
    {
        return InsertInternalAsync(
            entity,
            cancellationToken,
            key1,
            key2,
            key3,
            key4,
            key5);
    }

    public Task<TEntity> UpdateByComplexKeyAsync(
        TKey1 key1,
        TKey2 key2,
        TKey3 key3,
        TKey4 key4,
        TKey5 key5,
        TEntity entity,
        CancellationToken cancellationToken)
    {
        return UpdateInternalAsync(
            entity,
            cancellationToken,
            key1,
            key2,
            key3,
            key4,
            key5);
    }

    public Task DeleteByComplexKeyAsync(
        TKey1 key1,
        TKey2 key2,
        TKey3 key3,
        TKey4 key4,
        TKey5 key5,
        CancellationToken cancellationToken)
    {
        return DeleteInternalAsync(
            cancellationToken,
            key1,
            key2,
            key3,
            key4,
            key5);
    }
}