using AutoBackend.Sdk.Data.Storage;
using AutoBackend.Sdk.Filters;

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
> : GenericRepository<
    TEntity,
    TFilter
>, IGenericRepositoryWithComplexKey<
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
    where TEntity : class
    where TFilter : class, IGenericFilter
{
    public GenericRepositoryWithComplexKey(IGenericStorage<TEntity, TFilter> genericStorage) : base(genericStorage)
    {
    }

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

    public Task<TEntity> InsertByComplexKeyAsync(
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
        return InsertInternalAsync(
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