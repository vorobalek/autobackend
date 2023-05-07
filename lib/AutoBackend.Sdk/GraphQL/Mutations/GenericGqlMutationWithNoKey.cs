using AutoBackend.Sdk.Data.Repositories;
using AutoBackend.Sdk.Filters;
using AutoBackend.Sdk.Services.CancellationTokenProvider;

namespace AutoBackend.Sdk.GraphQL.Mutations;

internal abstract class GenericGqlMutationWithNoKey<
    TEntity,
    TFilter
> : GenericGqlMutation<
    TEntity,
    TFilter
>
    where TEntity : class
    where TFilter : class, IGenericFilter
{
    [GraphQLName("create")]
    public Task<TEntity> CreateAsync(
        [Service(ServiceKind.Resolver)] IGenericRepositoryWithNoKey<TEntity, TFilter> genericRepository,
        [Service(ServiceKind.Resolver)] ICancellationTokenProvider cancellationTokenProvider,
        [GraphQLName("entity")] TEntity entity)
    {
        return genericRepository
            .CreateAsync(
                entity,
                cancellationTokenProvider.GlobalCancellationToken);
    }
}