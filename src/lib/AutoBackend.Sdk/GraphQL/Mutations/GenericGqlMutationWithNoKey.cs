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
> : GenericGqlMutation<
    TEntity,
    TRequest,
    TResponse,
    TFilter
>
    where TEntity : class, new()
    where TRequest : class, IGenericRequest
    where TResponse : class, IGenericResponse, new()
    where TFilter : class, IGenericFilter
{
    [GraphQLName("create")]
    public async Task<TResponse> CreateAsync(
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