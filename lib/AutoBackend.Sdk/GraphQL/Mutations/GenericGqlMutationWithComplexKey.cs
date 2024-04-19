using AutoBackend.Sdk.Data.Mappers;
using AutoBackend.Sdk.Data.Repositories;
using AutoBackend.Sdk.Filters;
using AutoBackend.Sdk.Models;
using AutoBackend.Sdk.Services.CancellationTokenProvider;

namespace AutoBackend.Sdk.GraphQL.Mutations;

internal abstract class GenericGqlMutationWithComplexKey<
    TEntity,
    TRequest,
    TResponse,
    TFilter,
    TKey1,
    TKey2
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
    where TKey1 : notnull
    where TKey2 : notnull
{
    [GraphQLName("create")]
    public async Task<TResponse> CreateByComplexKeyAsync(
        [Service(ServiceKind.Resolver)] IGenericRequestMapper<TEntity, TRequest> genericRequestMapper,
        [Service(ServiceKind.Resolver)] IGenericResponseMapper<TEntity, TResponse> genericResponseMapper,
        [Service(ServiceKind.Resolver)] IGenericRepositoryWithComplexKey<
            TEntity,
            TFilter,
            TKey1,
            TKey2
        > genericRepository,
        [Service(ServiceKind.Resolver)] ICancellationTokenProvider cancellationTokenProvider,
        [GraphQLName("key1")] TKey1 key1,
        [GraphQLName("key2")] TKey2 key2,
        [GraphQLName("request")] TRequest request)
    {
        return genericResponseMapper
            .ToModel(
                await genericRepository
                    .CreateByComplexKeyAsync(
                        key1,
                        key2,
                        genericRequestMapper.ToEntity(request),
                        cancellationTokenProvider.GlobalCancellationToken));
    }

    [GraphQLName("update")]
    public async Task<TResponse> UpdateByComplexKeyAsync(
        [Service(ServiceKind.Resolver)] IGenericRequestMapper<TEntity, TRequest> genericRequestMapper,
        [Service(ServiceKind.Resolver)] IGenericResponseMapper<TEntity, TResponse> genericResponseMapper,
        [Service(ServiceKind.Resolver)] IGenericRepositoryWithComplexKey<
            TEntity,
            TFilter,
            TKey1,
            TKey2
        > genericRepository,
        [Service(ServiceKind.Resolver)] ICancellationTokenProvider cancellationTokenProvider,
        [GraphQLName("key1")] TKey1 key1,
        [GraphQLName("key2")] TKey2 key2,
        [GraphQLName("request")] TRequest request)
    {
        return genericResponseMapper
            .ToModel(
                await genericRepository
                    .UpdateByComplexKeyAsync(
                        key1,
                        key2,
                        genericRequestMapper.ToEntity(request),
                        cancellationTokenProvider.GlobalCancellationToken));
    }

    [GraphQLName("delete")]
    public async Task<bool> DeleteByComplexKeyAsync(
        [Service(ServiceKind.Resolver)] IGenericRepositoryWithComplexKey<
            TEntity,
            TFilter,
            TKey1,
            TKey2
        > genericRepository,
        [Service(ServiceKind.Resolver)] ICancellationTokenProvider cancellationTokenProvider,
        [GraphQLName("key1")] TKey1 key1,
        [GraphQLName("key2")] TKey2 key2)
    {
        await genericRepository
            .DeleteByComplexKeyAsync(
                key1,
                key2,
                cancellationTokenProvider.GlobalCancellationToken);
        return true;
    }
}