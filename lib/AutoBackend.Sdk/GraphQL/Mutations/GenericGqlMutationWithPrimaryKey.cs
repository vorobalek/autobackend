using AutoBackend.Sdk.Data.Mappers;
using AutoBackend.Sdk.Data.Repositories;
using AutoBackend.Sdk.Filters;
using AutoBackend.Sdk.Models;
using AutoBackend.Sdk.Services.CancellationTokenProvider;

namespace AutoBackend.Sdk.GraphQL.Mutations;

internal abstract class GenericGqlMutationWithPrimaryKey<
    TEntity,
    TRequest,
    TResponse,
    TFilter,
    TKey
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
    where TKey : notnull
{
    [GraphQLName("create")]
    public async Task<TResponse> CreateByPrimaryKeyAsync(
        [Service(ServiceKind.Resolver)] IGenericRequestMapper<TEntity, TRequest> genericRequestMapper,
        [Service(ServiceKind.Resolver)] IGenericResponseMapper<TEntity, TResponse> genericResponseMapper,
        [Service(ServiceKind.Resolver)] IGenericRepositoryWithPrimaryKey<TEntity, TFilter, TKey> genericRepository,
        [Service(ServiceKind.Resolver)] ICancellationTokenProvider cancellationTokenProvider,
        [GraphQLName("key")] TKey key,
        [GraphQLName("request")] TRequest request)
    {
        return genericResponseMapper
            .ToModel(
                await genericRepository
                    .CreateByPrimaryKeyAsync(
                        key,
                        genericRequestMapper.ToEntity(request),
                        cancellationTokenProvider.GlobalCancellationToken));
    }

    [GraphQLName("update")]
    public async Task<TResponse> UpdateByPrimaryKeyAsync(
        [Service(ServiceKind.Resolver)] IGenericRequestMapper<TEntity, TRequest> genericRequestMapper,
        [Service(ServiceKind.Resolver)] IGenericResponseMapper<TEntity, TResponse> genericResponseMapper,
        [Service(ServiceKind.Resolver)] IGenericRepositoryWithPrimaryKey<TEntity, TFilter, TKey> genericRepository,
        [Service(ServiceKind.Resolver)] ICancellationTokenProvider cancellationTokenProvider,
        [GraphQLName("key")] TKey key,
        [GraphQLName("request")] TRequest request)
    {
        return genericResponseMapper
            .ToModel(
                await genericRepository
                    .UpdateByPrimaryKeyAsync(
                        key,
                        genericRequestMapper.ToEntity(request),
                        cancellationTokenProvider.GlobalCancellationToken));
    }

    [GraphQLName("delete")]
    public async Task<bool> DeleteByPrimaryKeyAsync(
        [Service(ServiceKind.Resolver)] IGenericRepositoryWithPrimaryKey<TEntity, TFilter, TKey> genericRepository,
        [Service(ServiceKind.Resolver)] ICancellationTokenProvider cancellationTokenProvider,
        [GraphQLName("key")] TKey key)
    {
        await genericRepository
            .DeleteByPrimaryKeyAsync(
                key,
                cancellationTokenProvider.GlobalCancellationToken);
        return true;
    }
}