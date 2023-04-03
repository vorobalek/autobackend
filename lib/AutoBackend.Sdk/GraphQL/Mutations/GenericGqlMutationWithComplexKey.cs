using AutoBackend.Sdk.Data.Repositories;
using AutoBackend.Sdk.Filters;
using AutoBackend.Sdk.Services.CancellationTokenProvider;

namespace AutoBackend.Sdk.GraphQL.Mutations;

internal abstract class GenericGqlMutationWithComplexKey<
    TEntity,
    TFilter,
    TKey1,
    TKey2
> : GenericGqlMutation<
    TEntity,
    TFilter
>
    where TEntity : class
    where TFilter : class, IGenericFilter
    where TKey1 : notnull
    where TKey2 : notnull
{
    [GraphQLName("create")]
    public Task<TEntity> CreateByComplexKeyAsync(
        [Service(ServiceKind.Resolver)] IGenericRepositoryWithComplexKey<
            TEntity,
            TFilter,
            TKey1,
            TKey2
        > genericRepository,
        [Service(ServiceKind.Resolver)] ICancellationTokenProvider cancellationTokenProvider,
        [GraphQLName("key1")] TKey1 key1,
        [GraphQLName("key2")] TKey2 key2,
        [GraphQLName("entity")] TEntity entity)
    {
        return genericRepository
            .CreateByComplexKeyAsync(
                key1,
                key2,
                entity,
                cancellationTokenProvider.GlobalCancellationToken);
    }

    [GraphQLName("update")]
    public Task<TEntity> UpdateByComplexKeyAsync(
        [Service(ServiceKind.Resolver)] IGenericRepositoryWithComplexKey<
            TEntity,
            TFilter,
            TKey1,
            TKey2
        > genericRepository,
        [Service(ServiceKind.Resolver)] ICancellationTokenProvider cancellationTokenProvider,
        [GraphQLName("key1")] TKey1 key1,
        [GraphQLName("key2")] TKey2 key2,
        [GraphQLName("entity")] TEntity entity)
    {
        return genericRepository
            .UpdateByComplexKeyAsync(
                key1,
                key2,
                entity,
                cancellationTokenProvider.GlobalCancellationToken);
    }

    [GraphQLName("delete")]
    public async Task<bool> DeleteByComplexKeyAsync(
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
        await genericRepository
            .DeleteByComplexKeyAsync(
                key1,
                key2,
                cancellationTokenProvider.GlobalCancellationToken);
        return true;
    }
}