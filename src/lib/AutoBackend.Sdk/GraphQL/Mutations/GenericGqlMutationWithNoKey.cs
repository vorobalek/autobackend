using AutoBackend.Sdk.Data.Mappers;
using AutoBackend.Sdk.Data.Repositories;
using AutoBackend.Sdk.Filters;
using AutoBackend.Sdk.Models;
using AutoBackend.Sdk.Services.CancellationTokenProvider;

namespace AutoBackend.Sdk.GraphQL.Mutations;

internal abstract class GenericGqlMutationWithNoKey<
    TEntity,
    TRequest,
    TResponse,
    TFilter
> : GenericGqlMutation
    where TEntity : class, new()
    where TRequest : class, IGenericRequest
    where TResponse : class, IGenericResponse, new()
    where TFilter : class, IGenericFilter
{
    [GraphQLName("create")]
    // ReSharper disable once UnusedMember.Global
#pragma warning disable CA1822
    public async Task<TResponse> CreateAsync(
#pragma warning restore CA1822
        [Service] IGenericRequestMapper genericRequestMapper,
        [Service] IGenericResponseMapper genericResponseMapper,
        [Service] IGenericRepositoryWithNoKey<TEntity, TFilter> genericRepository,
        [Service] ICancellationTokenProvider cancellationTokenProvider,
        [GraphQLName("request")] TRequest request)
    {
        return genericResponseMapper
            .ToModel<TEntity, TResponse>(
                await genericRepository
                    .CreateAsync(
                        genericRequestMapper.ToEntity<TEntity, TRequest>(request),
                        cancellationTokenProvider.GlobalToken));
    }
}