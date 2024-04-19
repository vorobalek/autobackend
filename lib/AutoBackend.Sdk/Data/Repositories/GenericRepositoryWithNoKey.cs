using AutoBackend.Sdk.Data.Storage;
using AutoBackend.Sdk.Filters;

namespace AutoBackend.Sdk.Data.Repositories;

internal sealed class GenericRepositoryWithNoKey<
    TEntity,
    TFilter
>(IGenericStorage<TEntity, TFilter> genericStorage) : GenericRepository<
    TEntity,
    TFilter
>(genericStorage), IGenericRepositoryWithNoKey<
    TEntity,
    TFilter
>
    where TEntity : class
    where TFilter : class, IGenericFilter
{
    public Task<TEntity> CreateAsync(TEntity entity, CancellationToken cancellationToken)
    {
        return CreateInternalAsync(entity, cancellationToken);
    }
}