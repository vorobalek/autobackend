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
> : GenericGqlMutation
    where TEntity : class, new()
    where TRequest : class, IGenericRequest
    where TResponse : class, IGenericResponse, new()
    where TFilter : class, IGenericFilter
    where TKey : notnull
{
    [GraphQLName("create")]
    // ReSharper disable once UnusedMember.Global
#pragma warning disable CA1822
    public async Task<TResponse> CreateByPrimaryKeyAsync(
#pragma warning restore CA1822
        [Service] IGenericRequestMapper genericRequestMapper,
        [Service] IGenericResponseMapper genericResponseMapper,
        [Service] IGenericRepositoryWithPrimaryKey<TEntity, TFilter, TKey> genericRepository,
        [Service] ICancellationTokenProvider cancellationTokenProvider,
        [GraphQLName("key")] TKey key,
        [GraphQLName("request")] TRequest request)
    {
        return genericResponseMapper
            .ToModel<TEntity, TResponse>(
                await genericRepository
                    .CreateByPrimaryKeyAsync(
                        key,
                        genericRequestMapper.ToEntity<TEntity, TRequest>(request),
                        cancellationTokenProvider.GlobalToken));
    }

    [GraphQLName("update")]
    // ReSharper disable once UnusedMember.Global
#pragma warning disable CA1822
    public async Task<TResponse> UpdateByPrimaryKeyAsync(
#pragma warning restore CA1822
        [Service] IGenericRequestMapper genericRequestMapper,
        [Service] IGenericResponseMapper genericResponseMapper,
        [Service] IGenericRepositoryWithPrimaryKey<TEntity, TFilter, TKey> genericRepository,
        [Service] ICancellationTokenProvider cancellationTokenProvider,
        [GraphQLName("key")] TKey key,
        [GraphQLName("request")] TRequest request)
    {
        return genericResponseMapper
            .ToModel<TEntity, TResponse>(
                await genericRepository
                    .UpdateByPrimaryKeyAsync(
                        key,
                        genericRequestMapper.ToEntity<TEntity, TRequest>(request),
                        cancellationTokenProvider.GlobalToken));
    }

    [GraphQLName("delete")]
    // ReSharper disable once UnusedMember.Global
#pragma warning disable CA1822
    public async Task<bool> DeleteByPrimaryKeyAsync(
#pragma warning restore CA1822
        [Service] IGenericRepositoryWithPrimaryKey<TEntity, TFilter, TKey> genericRepository,
        [Service] ICancellationTokenProvider cancellationTokenProvider,
        [GraphQLName("key")] TKey key)
    {
        await genericRepository
            .DeleteByPrimaryKeyAsync(
                key,
                cancellationTokenProvider.GlobalToken);
        return true;
    }
}