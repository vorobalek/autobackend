using AutoBackend.Sdk.Data.Repositories;
using AutoBackend.Sdk.Filters;
using AutoBackend.Sdk.Models;
using AutoBackend.Sdk.Services.CancellationTokenProvider;

namespace AutoBackend.Sdk.GraphQL.Queries;

internal abstract class GenericGqlQueryWithComplexKey<
    TEntity,
    TResponse,
    TFilter,
    TKey1,
    TKey2,
    TKey3,
    TKey4
> : GenericGqlQuery<
    TEntity,
    TResponse,
    TFilter
>
    where TEntity : class
    where TResponse : class, IGenericResponse, new()
    where TFilter : class, IGenericFilter
    where TKey1 : notnull
    where TKey2 : notnull
    where TKey3 : notnull
    where TKey4 : notnull
{
    [GraphQLName("byKey")]
    public Task<TEntity?> GetByPrimaryKeyAsync(
        [Service] IGenericRepositoryWithComplexKey<
            TEntity,
            TFilter,
            TKey1,
            TKey2,
            TKey3,
            TKey4
        > genericRepository,
        [Service] ICancellationTokenProvider cancellationTokenProvider,
        [GraphQLName("key1")] TKey1 key1,
        [GraphQLName("key2")] TKey2 key2,
        [GraphQLName("key3")] TKey3 key3,
        [GraphQLName("key4")] TKey4 key4)
    {
        return genericRepository
            .GetByComplexKeyAsync(
                key1,
                key2,
                key3,
                key4,
                cancellationTokenProvider.GlobalToken);
    }
}