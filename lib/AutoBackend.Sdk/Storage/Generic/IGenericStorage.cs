namespace AutoBackend.Sdk.Storage.Generic;

internal interface IGenericStorage<TEntity> where TEntity : class
{
    Task<TEntity[]> GetAllAsync(CancellationToken cancellationToken = default);

    Task<TEntity[]> GetByFilterAsync<TFilter>(TFilter? filter, CancellationToken cancellationToken)
        where TFilter : class;

    Task<int> CountByFilterAsync<TFilter>(TFilter? filter, CancellationToken cancellationToken)
        where TFilter : class;
}