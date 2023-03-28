namespace AutoBackend.Sdk.Storage;

internal interface IGenericStorageWithFilter<TEntity, in TFilter> : IGenericStorage<TEntity>
    where TEntity : class
    where TFilter : class
{
    Task<TEntity[]> GetAllByFilterAsync(TFilter? filter, CancellationToken cancellationToken = default);

    Task<TEntity[]> GetSliceByFilterAsync(TFilter? filter, int? skipCount, int? takeCount, CancellationToken cancellationToken = default);

    Task<long> GetCountByFilterAsync(TFilter? filter, CancellationToken cancellationToken = default);
}