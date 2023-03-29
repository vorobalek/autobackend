using AutoBackend.Sdk.Data.Storage;
using AutoBackend.Sdk.Filters;

namespace AutoBackend.Sdk.Data.Repositories;

internal sealed class GenericRepositoryWithComplexKey<
    TEntity,
    TFilter,
    TKey1,
    TKey2
> : GenericRepository<
    TEntity,
    TFilter
>, IGenericRepositoryWithComplexKey<
    TEntity,
    TFilter,
    TKey1,
    TKey2
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
        CancellationToken cancellationToken)
    {
        return GetByKeyInternalAsync(
            cancellationToken,
            key1,
            key2);
    }

    public Task<TEntity> InsertByComplexKeyAsync(
        TKey1 key1,
        TKey2 key2,
        TEntity entity,
        CancellationToken cancellationToken)
    {
        return InsertInternalAsync(
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