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
    where TEntity : class
    where TResponse : class, IGenericResponse, new()
    where TFilter : class, IGenericFilter
    where TKey : notnull
{
    [GraphQLName("byKey")]
    public async Task<TResponse?> GetByPrimaryKeyAsync(
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