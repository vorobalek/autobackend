using AutoBackend.Sdk.Data.Mappers;
using AutoBackend.Sdk.Data.Repositories;
using AutoBackend.Sdk.Filters;
using AutoBackend.Sdk.Models;
using AutoBackend.Sdk.Services.CancellationTokenProvider;

namespace AutoBackend.Sdk.GraphQL.Queries;

internal abstract class GenericGqlQueryWithPrimaryKey<
    TEntity,
    TResponse,
    TFilter,
    TKey
> : GenericGqlQuery<
    TEntity,
    TResponse,
    TFilter
>
    where TEntity : class, new()
    where TResponse : class, IGenericResponse, new()
    where TFilter : class, IGenericFilter
    where TKey : notnull
{
    [GraphQLName("byKey")]
    // ReSharper disable once UnusedMember.Global
#pragma warning disable CA1822
    public async Task<TResponse?> GetByPrimaryKeyAsync(
#pragma warning restore CA1822
        [Service] IGenericResponseMapper genericResponseMapper,
        [Service] IGenericRepositoryWithPrimaryKey<TEntity, TFilter, TKey> genericRepository,
        [Service] ICancellationTokenProvider cancellationTokenProvider,
        [GraphQLName("key")] TKey key)
    {
        return await genericRepository
            .GetByPrimaryKeyAsync(
                key,
                cancellationTokenProvider.GlobalToken) is { } entity
            ? genericResponseMapper.ToModel<TEntity, TResponse>(entity)
            : null;
    }
}