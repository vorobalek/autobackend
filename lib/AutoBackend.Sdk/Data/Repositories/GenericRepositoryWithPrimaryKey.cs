using AutoBackend.Sdk.Data.Storage;
using AutoBackend.Sdk.Filters;

namespace AutoBackend.Sdk.Data.Repositories;

internal sealed class GenericRepositoryWithPrimaryKey<
    TEntity,
    TFilter,
    TKey
> : GenericRepository<
    TEntity,
    TFilter
>, IGenericRepositoryWithPrimaryKey<
    TEntity,
    TFilter,
    TKey
>
    where TEntity : class
    where TFilter : class, IGenericFilter
{
    public GenericRepositoryWithPrimaryKey(IGenericStorage<TEntity, TFilter> genericStorage) : base(genericStorage)
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