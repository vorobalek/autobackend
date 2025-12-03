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
    TKey5
> : GenericGqlMutation
    where TEntity : class, new()
    where TRequest : class, IGenericRequest
    where TResponse : class, IGenericResponse, new()
    where TFilter : class, IGenericFilter
    where TKey1 : notnull
    where TKey2 : notnull
    where TKey3 : notnull
    where TKey4 : notnull
    where TKey5 : notnull
{
    [GraphQLName("create")]
    // ReSharper disable once UnusedMember.Global
#pragma warning disable CA1822
    public async Task<TResponse> CreateByComplexKeyAsync(
#pragma warning restore CA1822
        [Service] IGenericRequestMapper genericRequestMapper,
        [Service] IGenericResponseMapper genericResponseMapper,
        [Service] IGenericRepositoryWithComplexKey<
            TEntity,
            TFilter,
            TKey1,
            TKey2,
            TKey3,
            TKey4,
            TKey5
        > genericRepository,
        [Service] ICancellationTokenProvider cancellationTokenProvider,
        [GraphQLName("key1")] TKey1 key1,
        [GraphQLName("key2")] TKey2 key2,
        [GraphQLName("key3")] TKey3 key3,
        [GraphQLName("key4")] TKey4 key4,
        [GraphQLName("key5")] TKey5 key5,
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
                        key5,
                        genericRequestMapper.ToEntity<TEntity, TRequest>(request),
                        cancellationTokenProvider.GlobalToken));
    }

    [GraphQLName("update")]
    // ReSharper disable once UnusedMember.Global
#pragma warning disable CA1822
    public async Task<TResponse> UpdateByComplexKeyAsync(
#pragma warning restore CA1822
        [Service] IGenericRequestMapper genericRequestMapper,
        [Service] IGenericResponseMapper genericResponseMapper,
        [Service] IGenericRepositoryWithComplexKey<
            TEntity,
            TFilter,
            TKey1,
            TKey2,
            TKey3,
            TKey4,
            TKey5
        > genericRepository,
        [Service] ICancellationTokenProvider cancellationTokenProvider,
        [GraphQLName("key1")] TKey1 key1,
        [GraphQLName("key2")] TKey2 key2,
        [GraphQLName("key3")] TKey3 key3,
        [GraphQLName("key4")] TKey4 key4,
        [GraphQLName("key5")] TKey5 key5,
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
                        key5,
                        genericRequestMapper.ToEntity<TEntity, TRequest>(request),
                        cancellationTokenProvider.GlobalToken));
    }

    [GraphQLName("delete")]
    // ReSharper disable once UnusedMember.Global
#pragma warning disable CA1822
    public async Task<bool> DeleteByComplexKeyAsync(
#pragma warning restore CA1822
        [Service] IGenericRepositoryWithComplexKey<
            TEntity,
            TFilter,
            TKey1,
            TKey2,
            TKey3,
            TKey4,
            TKey5
        > genericRepository,
        [Service] ICancellationTokenProvider cancellationTokenProvider,
        [GraphQLName("key1")] TKey1 key1,
        [GraphQLName("key2")] TKey2 key2,
        [GraphQLName("key3")] TKey3 key3,
        [GraphQLName("key4")] TKey4 key4,
        [GraphQLName("key5")] TKey5 key5)
    {
        await genericRepository
            .DeleteByComplexKeyAsync(
                key1,
                key2,
                key3,
                key4,
                key5,
                cancellationTokenProvider.GlobalToken);
        return true;
    }
}