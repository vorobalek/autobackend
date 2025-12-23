using AutoBackend.Sdk.Data.Storage;
using AutoBackend.Sdk.Filters;
using AutoBackend.Sdk.Services.PermissionValidator;

namespace AutoBackend.Sdk.Data.Repositories;

internal sealed class GenericRepositoryWithComplexKey<
    TEntity,
    TFilter,
    TKey1,
    TKey2
>(IPermissionValidator permissionValidator, IGenericStorage<TEntity, TFilter> genericStorage) : GenericRepository<
    TEntity,
    TFilter
>(permissionValidator, genericStorage), IGenericRepositoryWithComplexKey<
    TEntity,
    TFilter,
    TKey1,
    TKey2
>
    where TEntity : class, new()
    where TFilter : class, IGenericFilter
    where TKey1 : notnull
    where TKey2 : notnull
{
    public Task<TEntity?> GetByComplexKeyAsync(
        TKey1 key1,
        TKey2 key2,
        CancellationToken cancellationToken)
    {
        return GetByKeyInternalAsync(
            cancellationToken,
            key1,
            key2);
    }

    public Task<TEntity> CreateByComplexKeyAsync(
        TKey1 key1,
        TKey2 key2,
        TEntity entity,
        CancellationToken cancellationToken)
    {
        return CreateInternalAsync(
            entity,
            cancellationToken,
            key1,
            key2);
    }

    public Task<TEntity> UpdateByComplexKeyAsync(
        TKey1 key1,
        TKey2 key2,
        TEntity entity,
        CancellationToken cancellationToken)
    {
        return UpdateInternalAsync(
            entity,
            cancellationToken,
            key1,
            key2);
    }

    public Task DeleteByComplexKeyAsync(
        TKey1 key1,
        TKey2 key2,
        CancellationToken cancellationToken)
    {
        return DeleteInternalAsync(
            cancellationToken,
            key1,
            key2);
    }
}