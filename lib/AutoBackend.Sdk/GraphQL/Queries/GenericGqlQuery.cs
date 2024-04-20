using AutoBackend.Sdk.Data.Mappers;
using AutoBackend.Sdk.Data.Repositories;
using AutoBackend.Sdk.Filters;
using AutoBackend.Sdk.Models;
using AutoBackend.Sdk.Services.CancellationTokenProvider;

namespace AutoBackend.Sdk.GraphQL.Queries;

internal abstract class GenericGqlQuery<
    TEntity,
    TResponse,
    TFilter
>
    where TEntity : class
    where TResponse : class, IGenericResponse, new()
    where TFilter : class, IGenericFilter
{
    [GraphQLName("all")]
    [UseProjection]
    public async Task<TResponse[]> GetAllAsync(
        [Service(ServiceKind.Resolver)] IGenericResponseMapper genericResponseMapper,
        [Service(ServiceKind.Resolver)] IGenericRepository<TEntity, TFilter> genericRepository,
        [Service(ServiceKind.Resolver)] ICancellationTokenProvider cancellationTokenProvider,
        [GraphQLName("filter")] TFilter? filter)
    {
        return genericResponseMapper
            .ToModel<TEntity, TResponse>(
                await genericRepository
                    .GetAllAsync(
                        filter,
                        cancellationTokenProvider.GlobalCancellationToken))
            .ToArray();
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