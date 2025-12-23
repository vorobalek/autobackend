using AutoBackend.Sdk.Data.Storage;
using AutoBackend.Sdk.Filters;
using AutoBackend.Sdk.Services.PermissionValidator;

namespace AutoBackend.Sdk.Data.Repositories;

internal sealed class GenericRepositoryWithNoKey<
    TEntity,
    TFilter
>(IPermissionValidator permissionValidator, IGenericStorage<TEntity, TFilter> genericStorage) : GenericRepository<
    TEntity,
    TFilter
>(permissionValidator, genericStorage), IGenericRepositoryWithNoKey<
    TEntity,
    TFilter
>
    where TEntity : class, new()
    where TFilter : class, IGenericFilter
{
    public Task<TEntity> CreateAsync(TEntity entity, CancellationToken cancellationToken)
    {
        return CreateInternalAsync(entity, cancellationToken);
    }
}