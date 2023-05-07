using AutoBackend.Sdk.Filters;

namespace AutoBackend.Sdk.Data.Repositories;

internal interface IGenericRepository<TEntity, in TFilter>
    where TEntity : class
    where TFilter : class, IGenericFilter
{
    Task<TEntity[]> GetAllAsync(TFilter? filter, CancellationToken cancellationToken);

    Task<long> GetCountAsync(TFilter? filter, CancellationToken cancellationToken);
}