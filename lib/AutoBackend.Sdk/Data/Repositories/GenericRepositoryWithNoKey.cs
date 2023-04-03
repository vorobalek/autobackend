using AutoBackend.Sdk.Data.Storage;
using AutoBackend.Sdk.Filters;

namespace AutoBackend.Sdk.Data.Repositories;

internal sealed class GenericRepositoryWithNoKey<
    TEntity,
    TFilter
> : GenericRepository<
    TEntity,
    TFilter
>, IGenericRepositoryWithNoKey<
    TEntity,
    TFilter
>
    where TEntity : class
    where TFilter : class, IGenericFilter
{
    public GenericRepositoryWithNoKey(IGenericStorage<TEntity, TFilter> genericStorage) : base(genericStorage)
    {
    }

    public Task<TEntity> CreateAsync(TEntity entity, CancellationToken cancellationToken)
    {
        return CreateInternalAsync(entity, cancellationToken);
    }
}