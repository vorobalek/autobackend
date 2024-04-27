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
    TKey3,
    TKey4
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
    where TKey4 : notnull
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
            TKey3,
            TKey4
        > genericRepository,
        [Service(ServiceKind.Resolver)] ICancellationTokenProvider cancellationTokenProvider,
        [GraphQLName("key1")] TKey1 key1,
        [GraphQLName("key2")] TKey2 key2,
        [GraphQLName("key3")] TKey3 key3,
        [GraphQLName("key4")] TKey4 key4,
        [GraphQLName("request")] TRequest request)
    {
        return genericResponseMapper
            .ToModel<TEntity, TResponse>(
                await genericRepository
                    .CreateByComplexKeyAsync(
                        key1,
                        key2,
                        key3,
                        key4,
                        genericRequestMapper.ToEntity<TEntity, TRequest>(request),
                        cancellationTokenProvider.GlobalToken));
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
            TKey3,
            TKey4
        > genericRepository,
        [Service(ServiceKind.Resolver)] ICancellationTokenProvider cancellationTokenProvider,
        [GraphQLName("key1")] TKey1 key1,
        [GraphQLName("key2")] TKey2 key2,
        [GraphQLName("key3")] TKey3 key3,
        [GraphQLName("key4")] TKey4 key4,
        [GraphQLName("request")] TRequest request)
    {
        return genericResponseMapper
            .ToModel<TEntity, TResponse>(
                await genericRepository
                    .UpdateByComplexKeyAsync(
                        key1,
                        key2,
                        key3,
                        key4,
                        genericRequestMapper.ToEntity<TEntity, TRequest>(request),
                        cancellationTokenProvider.GlobalToken));
    }

    [GraphQLName("delete")]
    public async Task<bool> DeleteByComplexKeyAsync(
        [Service(ServiceKind.Resolver)] IGenericRepositoryWithComplexKey<
            TEntity,
            TFilter,
            TKey1,
            TKey2,
            TKey3,
            TKey4
        > genericRepository,
        [Service(ServiceKind.Resolver)] ICancellationTokenProvider cancellationTokenProvider,
        [GraphQLName("key1")] TKey1 key1,
        [GraphQLName("key2")] TKey2 key2,
        [GraphQLName("key3")] TKey3 key3,
        [GraphQLName("key4")] TKey4 key4)
    {
        await genericRepository
            .DeleteByComplexKeyAsync(
                key1,
                key2,
                key3,
                key4,
                cancellationTokenProvider.GlobalToken);
        return true;
    }
}