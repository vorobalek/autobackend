using AutoBackend.Sdk.Data.Repositories;
using AutoBackend.Sdk.Data.Storage;
using AutoBackend.Sdk.Filters;
using AutoBackend.Sdk.Services.CancellationTokenProvider;

namespace AutoBackend.Sdk.GraphQL.Queries;

internal abstract class GenericGqlQuery<
    TEntity,
    TFilter
>
    where TEntity : class
    where TFilter : class, IGenericFilter
{
    [GraphQLName("all")]
    [UseProjection]
    public IQueryable<TEntity> GetAllList(
        [Service(ServiceKind.Resolver)] IGenericStorage<TEntity, TFilter> genericStorage,
        [GraphQLName("filter")] TFilter? filter)
    {
        return genericStorage.GetQuery(filter);
    }

    [GraphQLName("count")]
    public Task<long> GetCountAsync(
        [Service(ServiceKind.Resolver)] IGenericRepository<TEntity, TFilter> genericRepository,
        [Service(ServiceKind.Resolver)] ICancellationTokenProvider cancellationTokenProvider,
        [GraphQLName("filter")] TFilter? filter)
    {
        return genericRepository
            .GetCountAsync(
                filter,
                cancellationTokenProvider.GlobalCancellationToken);
    }
}