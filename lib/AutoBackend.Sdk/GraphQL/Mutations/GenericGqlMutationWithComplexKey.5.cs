using AutoBackend.Sdk.Data.Repositories;
using AutoBackend.Sdk.Filters;
using AutoBackend.Sdk.Services.CancellationTokenProvider;

namespace AutoBackend.Sdk.GraphQL.Mutations;

internal abstract class GenericGqlMutationWithComplexKey<
    TEntity,
    TFilter,
    TKey1,
    TKey2,
    TKey3,
    TKey4,
    TKey5
> : GenericGqlMutation<
    TEntity,
    TFilter
>
    where TEntity : class
    where TFilter : class, IGenericFilter
    where TKey1 : notnull
    where TKey2 : notnull
    where TKey3 : notnull
    where TKey4 : notnull
    where TKey5 : notnull
{
    [GraphQLName("create")]
    public Task<TEntity> CreateByComplexKeyAsync(
        [Service(ServiceKind.Resolver)] IGenericRepositoryWithComplexKey<
            TEntity,
            TFilter,
            TKey1,
            TKey2,
            TKey3,
            TKey4,
            TKey5
        > genericRepository,
        [Service(ServiceKind.Resolver)] ICancellationTokenProvider cancellationTokenProvider,
        [GraphQLName("key1")] TKey1 key1,
        [GraphQLName("key2")] TKey2 key2,
        [GraphQLName("key3")] TKey3 key3,
        [GraphQLName("key4")] TKey4 key4,
        [GraphQLName("key5")] TKey5 key5,
        [GraphQLName("entity")] TEntity entity)
    {
        return genericRepository
            .CreateByComplexKeyAsync(
                key1,
                key2,
                key3,
                key4,
                key5,
                entity,
                cancellationTokenProvider.GlobalCancellationToken);
    }

    [GraphQLName("update")]
    public Task<TEntity> UpdateByComplexKeyAsync(
        [Service(ServiceKind.Resolver)] IGenericRepositoryWithComplexKey<
            TEntity,
            TFilter,
            TKey1,
            TKey2,
            TKey3,
            TKey4,
            TKey5
        > genericRepository,
        [Service(ServiceKind.Resolver)] ICancellationTokenProvider cancellationTokenProvider,
        [GraphQLName("key1")] TKey1 key1,
        [GraphQLName("key2")] TKey2 key2,
        [GraphQLName("key3")] TKey3 key3,
        [GraphQLName("key4")] TKey4 key4,
        [GraphQLName("key5")] TKey5 key5,
        [GraphQLName("entity")] TEntity entity)
    {
        return genericRepository
            .UpdateByComplexKeyAsync(
                key1,
                key2,
                key3,
                key4,
                key5,
                entity,
                cancellationTokenProvider.GlobalCancellationToken);
    }

    [GraphQLName("delete")]
    public async Task<bool> DeleteByComplexKeyAsync(
        [Service(ServiceKind.Resolver)] IGenericRepositoryWithComplexKey<
            TEntity,
            TFilter,
            TKey1,
            TKey2,
            TKey3,
            TKey4,
            TKey5
        > genericRepository,
        [Service(ServiceKind.Resolver)] ICancellationTokenProvider cancellationTokenProvider,
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
                cancellationTokenProvider.GlobalCancellationToken);
        return true;
    }
}