using AutoBackend.Sdk.Data.Storage;
using AutoBackend.Sdk.Filters;
using AutoBackend.Sdk.Services.PermissionValidator;

namespace AutoBackend.Sdk.Data.Repositories;

internal sealed class GenericRepositoryWithPrimaryKey<
    TEntity,
    TFilter,
    TKey
>(IPermissionValidator permissionValidator, IGenericStorage<TEntity, TFilter> genericStorage) : GenericRepository<
    TEntity,
    TFilter
>(permissionValidator, genericStorage), IGenericRepositoryWithPrimaryKey<
    TEntity,
    TFilter,
    TKey
>
    where TEntity : class, new()
    where TFilter : class, IGenericFilter
    where TKey : notnull
{
    public Task<TEntity?> GetByPrimaryKeyAsync(TKey key, CancellationToken cancellationToken)
    {
        return GetByKeyInternalAsync(cancellationToken, key);
    }

    public Task<TEntity> CreateByPrimaryKeyAsync(TKey key, TEntity entity, CancellationToken cancellationToken)
    {
        return CreateInternalAsync(entity, cancellationToken, key);
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