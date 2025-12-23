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
    where TEntity : class, new()
    where TResponse : class, IGenericResponse, new()
    where TFilter : class, IGenericFilter
{
    [GraphQLName("all")]
    [UseProjection]
    // ReSharper disable once UnusedMember.Global
#pragma warning disable CA1822
    public async Task<TResponse[]> GetAllAsync(
#pragma warning restore CA1822
        [Service] IGenericResponseMapper genericResponseMapper,
        [Service] IGenericRepository<TEntity, TFilter> genericRepository,
        [Service] ICancellationTokenProvider cancellationTokenProvider,
        [GraphQLName("filter")] TFilter? filter)
    {
        return genericResponseMapper
            .ToModel<TEntity, TResponse>(
                await genericRepository
                    .GetAllAsync(
                        filter,
                        cancellationTokenProvider.GlobalToken))
            .ToArray();
    }

    [GraphQLName("count")]
    // ReSharper disable once UnusedMember.Global
#pragma warning disable CA1822
    public Task<long> GetCountAsync(
#pragma warning restore CA1822
        [Service] IGenericRepository<TEntity, TFilter> genericRepository,
        [Service] ICancellationTokenProvider cancellationTokenProvider,
        [GraphQLName("filter")] TFilter? filter)
    {
        return genericRepository
            .GetCountAsync(
                filter,
                cancellationTokenProvider.GlobalToken);
    }
}