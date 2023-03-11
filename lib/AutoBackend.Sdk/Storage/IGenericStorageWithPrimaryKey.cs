namespace AutoBackend.Sdk.Storage;

internal interface IGenericStorageWithPrimaryKey<
    TEntity,
    in TKey
> : IGenericStorage<TEntity> where TEntity : class
{
    Task<TEntity?> GetByPrimaryKeyAsync(TKey key, CancellationToken cancellationToken);
    Task<TEntity> InsertByPrimaryKeyAsync(TKey key, TEntity entity, CancellationToken cancellationToken);
    Task<TEntity> UpdateByPrimaryKeyAsync(TKey key, TEntity entity, CancellationToken cancellationToken);
    Task DeleteByPrimaryKeyAsync(TKey key, CancellationToken cancellationToken);
}