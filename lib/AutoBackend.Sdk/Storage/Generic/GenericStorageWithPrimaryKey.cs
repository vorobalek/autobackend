using AutoBackend.Sdk.Data;

namespace AutoBackend.Sdk.Storage.Generic;

internal class GenericStorageWithPrimaryKey<
    TEntity,
    TKey
> : GenericStorage<
    TEntity
>, IGenericStorageWithPrimaryKey<
    TEntity,
    TKey
> where TEntity : class
{
    public GenericStorageWithPrimaryKey(AutoBackendDbContext db) : base(db)
    {
    }

    public Task<TEntity?> GetByPrimaryKeyAsync(TKey key, CancellationToken cancellationToken)
    {
        return GetByKeyInternalAsync(cancellationToken, key);
    }

    public Task<TEntity> InsertByPrimaryKeyAsync(TKey key, TEntity entity, CancellationToken cancellationToken)
    {
        return InsertInternalAsync(entity, cancellationToken, key);
    }

    public Task<TEntity> UpdateByPrimaryKeyAsync(TKey key, TEntity entity, CancellationToken cancellationToken)
    {
        return UpdateInternalAsync(entity, cancellationToken, key);
    }

    public Task DeleteByPrimaryKeyAsync(TKey key, CancellationToken cancellationToken)
    {
        return DeleteInternalAsync(cancellationToken, key);
    }
}