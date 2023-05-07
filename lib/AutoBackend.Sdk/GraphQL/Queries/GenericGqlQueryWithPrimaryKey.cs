using AutoBackend.Sdk.Data.Repositories;
using AutoBackend.Sdk.Filters;
using AutoBackend.Sdk.Services.CancellationTokenProvider;

namespace AutoBackend.Sdk.GraphQL.Queries;

internal abstract class GenericGqlQueryWithPrimaryKey<
    TEntity,
    TFilter,
    TKey
> : GenericGqlQuery<
    TEntity,
    TFilter
>
    where TEntity : class
    where TFilter : class, IGenericFilter
    where TKey : notnull
{
    [GraphQLName("byKey")]
    public Task<TEntity?> GetByPrimaryKeyAsync(
        [Service(ServiceKind.Resolver)] IGenericRepositoryWithPrimaryKey<TEntity, TFilter, TKey> genericRepository,
        [Service(ServiceKind.Resolver)] ICancellationTokenProvider cancellationTokenProvider,
        [GraphQLName("key")] TKey key)
    {
        return genericRepository
            .GetByPrimaryKeyAsync(
                key,
                cancellationTokenProvider.GlobalCancellationToken);
    }
}