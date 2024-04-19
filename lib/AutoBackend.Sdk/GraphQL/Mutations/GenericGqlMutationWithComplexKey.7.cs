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
    TKey4,
    TKey5,
    TKey6,
    TKey7
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
    where TKey5 : notnull
    where TKey6 : notnull
    where TKey7 : notnull
{
    [GraphQLName("create")]
    public async Task<TResponse> CreateByComplexKeyAsync(
        [Service(ServiceKind.Resolver)] IGenericRequestMapper<TEntity, TRequest> genericRequestMapper,
        [Service(ServiceKind.Resolver)] IGenericResponseMapper<TEntity, TResponse> genericResponseMapper,
        [Service(ServiceKind.Resolver)] IGenericRepositoryWithComplexKey<
            TEntity,
            TFilter,
            TKey1,
            TKey2,
            TKey3,
            TKey4,
            TKey5,
            TKey6,
            TKey7
        > genericRepository,
        [Service(ServiceKind.Resolver)] ICancellationTokenProvider cancellationTokenProvider,
        [GraphQLName("key1")] TKey1 key1,
        [GraphQLName("key2")] TKey2 key2,
        [GraphQLName("key3")] TKey3 key3,
        [GraphQLName("key4")] TKey4 key4,
        [GraphQLName("key5")] TKey5 key5,
        [GraphQLName("key6")] TKey6 key6,
        [GraphQLName("key7")] TKey7 key7,
        [GraphQLName("request")] TRequest request)
    {
        return genericResponseMapper
            .ToModel(
                await genericRepository
                    .CreateByComplexKeyAsync(
                        key1,
                        key2,
                        key3,
                        key4,
                        key5,
                        key6,
                        key7,
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
            TKey2,
            TKey3,
            TKey4,
            TKey5,
            TKey6,
            TKey7
        > genericRepository,
        [Service(ServiceKind.Resolver)] ICancellationTokenProvider cancellationTokenProvider,
        [GraphQLName("key1")] TKey1 key1,
        [GraphQLName("key2")] TKey2 key2,
        [GraphQLName("key3")] TKey3 key3,
        [GraphQLName("key4")] TKey4 key4,
        [GraphQLName("key5")] TKey5 key5,
        [GraphQLName("key6")] TKey6 key6,
        [GraphQLName("key7")] TKey7 key7,
        [GraphQLName("request")] TRequest request)
    {
        return genericResponseMapper
            .ToModel(
                await genericRepository
                    .UpdateByComplexKeyAsync(
                        key1,
                        key2,
                        key3,
                        key4,
                        key5,
                        key6,
                        key7,
                        genericRequestMapper.ToEntity(request),
                        cancellationTokenProvider.GlobalCancellationToken));
    }

    [GraphQLName("delete")]
    public async Task<bool> DeleteByComplexKeyAsync(
        [Service(ServiceKind.Resolver)] IGenericRepositoryWithComplexKey<
            TEntity,
            TFilter,
            TKey1,
            TKey2,
            TKey3,
            TKey4,
            TKey5,
            TKey6,
            TKey7
        > genericRepository,
        [Service(ServiceKind.Resolver)] ICancellationTokenProvider cancellationTokenProvider,
        [GraphQLName("key1")] TKey1 key1,
        [GraphQLName("key2")] TKey2 key2,
        [GraphQLName("key3")] TKey3 key3,
        [GraphQLName("key4")] TKey4 key4,
        [GraphQLName("key5")] TKey5 key5,
        [GraphQLName("key6")] TKey6 key6,
        [GraphQLName("key7")] TKey7 key7)
    {
        await genericRepository
            .DeleteByComplexKeyAsync(
                key1,
                key2,
                key3,
                key4,
                key5,
                key6,
                key7,
                cancellationTokenProvider.GlobalCancellationToken);
        return true;
    }
}