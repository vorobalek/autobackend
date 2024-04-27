using AutoBackend.Sdk.Filters;

namespace AutoBackend.Sdk.Data.Repositories;

internal interface IGenericRepositoryWithNoKey<
    TEntity,
    in TFilter
> : IGenericRepository<
    TEntity,
    TFilter
>
    where TEntity : class
    where TFilter : class, IGenericFilter
{
    Task<TEntity> CreateAsync(TEntity entity, CancellationToken cancellationToken);
}