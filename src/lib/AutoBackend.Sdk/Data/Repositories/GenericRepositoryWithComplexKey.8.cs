using AutoBackend.Sdk.Data.Storage;
using AutoBackend.Sdk.Filters;
using AutoBackend.Sdk.Services.PermissionValidator;

namespace AutoBackend.Sdk.Data.Repositories;

internal sealed class GenericRepositoryWithComplexKey<
    TEntity,
    TFilter,
    TKey1,
    TKey2,
    TKey3,
    TKey4,
    TKey5,
    TKey6,
    TKey7,
    TKey8
>(IPermissionValidator permissionValidator, IGenericStorage<TEntity, TFilter> genericStorage) : GenericRepository<
    TEntity,
    TFilter
>(permissionValidator, genericStorage), IGenericRepositoryWithComplexKey<
    TEntity,
    TFilter,
    TKey1,
    TKey2,
    TKey3,
    TKey4,
    TKey5,
    TKey6,
    TKey7,
    TKey8
>
    where TEntity : class, new()
    where TFilter : class, IGenericFilter
    where TKey1 : notnull
    where TKey2 : notnull
    where TKey3 : notnull
    where TKey4 : notnull
    where TKey5 : notnull
    where TKey6 : notnull
    where TKey7 : notnull
    where TKey8 : notnull
{
    public Task<TEntity?> GetByComplexKeyAsync(
        TKey1 key1,
        TKey2 key2,
        TKey3 key3,
        TKey4 key4,
        TKey5 key5,
        TKey6 key6,
        TKey7 key7,
        TKey8 key8,
        CancellationToken cancellationToken)
    {
        return GetByKeyInternalAsync(
            cancellationToken,
            key1,
            key2,
            key3,
            key4,
            key5,
            key6,
            key7,
            key8);
    }

    public Task<TEntity> CreateByComplexKeyAsync(
        TKey1 key1,
        TKey2 key2,
        TKey3 key3,
        TKey4 key4,
        TKey5 key5,
        TKey6 key6,
        TKey7 key7,
        TKey8 key8,
        TEntity entity,
        CancellationToken cancellationToken)
    {
        return CreateInternalAsync(
            entity,
            cancellationToken,
            key1,
            key2,
            key3,
            key4,
            key5,
            key6,
            key7,
            key8);
    }

    public Task<TEntity> UpdateByComplexKeyAsync(
        TKey1 key1,
        TKey2 key2,
        TKey3 key3,
        TKey4 key4,
        TKey5 key5,
        TKey6 key6,
        TKey7 key7,
        TKey8 key8,
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
            key5,
            key6,
            key7,
            key8);
    }

    public Task DeleteByComplexKeyAsync(
        TKey1 key1,
        TKey2 key2,
        TKey3 key3,
        TKey4 key4,
        TKey5 key5,
        TKey6 key6,
        TKey7 key7,
        TKey8 key8,
        CancellationToken cancellationToken)
    {
        return DeleteInternalAsync(
            cancellationToken,
            key1,
            key2,
            key3,
            key4,
            key5,
            key6,
            key7,
            key8);
    }
}