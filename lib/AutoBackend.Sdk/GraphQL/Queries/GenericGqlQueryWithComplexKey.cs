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
    TKey2
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
{
    [GraphQLName("byKey")]
    public Task<TEntity?> GetByPrimaryKeyAsync(
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
        return genericRepository
            .GetByComplexKeyAsync(
                key1,
                key2,
                cancellationTokenProvider.GlobalCancellationToken);
    }
}