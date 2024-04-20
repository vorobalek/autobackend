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
    TKey2,
    TKey3
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
    where TKey3 : notnull
{
    [GraphQLName("create")]
    public async Task<TResponse> CreateByComplexKeyAsync(
        [Service(ServiceKind.Resolver)] IGenericRequestMapper genericRequestMapper,
        [Service(ServiceKind.Resolver)] IGenericResponseMapper genericResponseMapper,
        [Service(ServiceKind.Resolver)] IGenericRepositoryWithComplexKey<
            TEntity,
            TFilter,
            TKey1,
            TKey2,
            TKey3
        > genericRepository,
        [Service(ServiceKind.Resolver)] ICancellationTokenProvider cancellationTokenProvider,
        [GraphQLName("key1")] TKey1 key1,
        [GraphQLName("key2")] TKey2 key2,
        [GraphQLName("key3")] TKey3 key3,
        [GraphQLName("request")] TRequest request)
    {
        return genericResponseMapper
            .ToModel<TEntity, TResponse>(
                await genericRepository
                    .CreateByComplexKeyAsync(
                        key1,
                        key2,
                        key3,
                        genericRequestMapper.ToEntity<TEntity, TRequest>(request),
                        cancellationTokenProvider.GlobalCancellationToken));
    }

    [GraphQLName("update")]
    public async Task<TResponse> UpdateByComplexKeyAsync(
        [Service(ServiceKind.Resolver)] IGenericRequestMapper genericRequestMapper,
        [Service(ServiceKind.Resolver)] IGenericResponseMapper genericResponseMapper,
        [Service(ServiceKind.Resolver)] IGenericRepositoryWithComplexKey<
            TEntity,
            TFilter,
            TKey1,
            TKey2,
            TKey3
        > genericRepository,
        [Service(ServiceKind.Resolver)] ICancellationTokenProvider cancellationTokenProvider,
        [GraphQLName("key1")] TKey1 key1,
        [GraphQLName("key2")] TKey2 key2,
        [GraphQLName("key3")] TKey3 key3,
        [GraphQLName("request")] TRequest request)
    {
        return genericResponseMapper
            .ToModel<TEntity, TResponse>(
                await genericRepository
                    .UpdateByComplexKeyAsync(
                        key1,
                        key2,
                        key3,
                        genericRequestMapper.ToEntity<TEntity, TRequest>(request),
                        cancellationTokenProvider.GlobalCancellationToken));
    }

    [GraphQLName("delete")]
    public async Task<bool> DeleteByComplexKeyAsync(
        [Service(ServiceKind.Resolver)] IGenericRepositoryWithComplexKey<
            TEntity,
            TFilter,
            TKey1,
            TKey2,
            TKey3
        > genericRepository,
        [Service(ServiceKind.Resolver)] ICancellationTokenProvider cancellationTokenProvider,
        [GraphQLName("key1")] TKey1 key1,
        [GraphQLName("key2")] TKey2 key2,
        [GraphQLName("key3")] TKey3 key3)
    {
        await genericRepository
            .DeleteByComplexKeyAsync(
                key1,
                key2,
                key3,
                cancellationTokenProvider.GlobalCancellationToken);
        return true;
    }
}