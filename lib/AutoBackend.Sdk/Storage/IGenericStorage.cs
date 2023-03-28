namespace AutoBackend.Sdk.Storage;

internal interface IGenericStorage<TEntity> where TEntity : class
{
    Task<TEntity[]> GetAllAsync(CancellationToken cancellationToken = default);

    Task<TEntity[]> GetSliceAsync(int? skipCount, int? takeCount, CancellationToken cancellationToken = default);

    Task<long> CountAsync(CancellationToken cancellationToken);
}