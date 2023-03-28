namespace AutoBackend.Sdk.Storage;

internal interface IGenericFilteredStorage<TEntity, in TFilter> : IGenericStorage<TEntity>
    where TEntity : class
    where TFilter : class
{
    Task<TEntity[]> GetAllByFilterAsync(TFilter? filter, CancellationToken cancellationToken);

    Task<TEntity[]> GetSliceByFilterAsync(TFilter? filter, int? skipCount, int? takeCount, CancellationToken cancellationToken = default);

    Task<long> CountByFilterAsync(TFilter? filter, CancellationToken cancellationToken);
}