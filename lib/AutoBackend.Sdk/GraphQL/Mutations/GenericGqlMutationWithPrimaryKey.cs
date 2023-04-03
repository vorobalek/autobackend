using AutoBackend.Sdk.Data.Repositories;
using AutoBackend.Sdk.Filters;
using AutoBackend.Sdk.Services.CancellationTokenProvider;

namespace AutoBackend.Sdk.GraphQL.Mutations;

internal abstract class GenericGqlMutationWithPrimaryKey<
    TEntity,
    TFilter,
    TKey
> : GenericGqlMutation<
    TEntity,
    TFilter
>
    where TEntity : class
    where TFilter : class, IGenericFilter
    where TKey : notnull
{
    [GraphQLName("create")]
    public Task<TEntity> CreateByPrimaryKeyAsync(
        [Service(ServiceKind.Resolver)] IGenericRepositoryWithPrimaryKey<TEntity, TFilter, TKey> genericRepository,
        [Service(ServiceKind.Resolver)] ICancellationTokenProvider cancellationTokenProvider,
        [GraphQLName("key")] TKey key,
        [GraphQLName("entity")] TEntity entity)
    {
        return genericRepository
            .CreateByPrimaryKeyAsync(
                key,
                entity,
                cancellationTokenProvider.GlobalCancellationToken);
    }

    [GraphQLName("update")]
    public Task<TEntity> UpdateByPrimaryKeyAsync(
        [Service(ServiceKind.Resolver)] IGenericRepositoryWithPrimaryKey<TEntity, TFilter, TKey> genericRepository,
        [Service(ServiceKind.Resolver)] ICancellationTokenProvider cancellationTokenProvider,
        [GraphQLName("key")] TKey key,
        [GraphQLName("entity")] TEntity entity)
    {
        return genericRepository
            .UpdateByPrimaryKeyAsync(
                key,
                entity,
                cancellationTokenProvider.GlobalCancellationToken);
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