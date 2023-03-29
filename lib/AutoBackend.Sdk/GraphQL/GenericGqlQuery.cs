using AutoBackend.Sdk.Data.Repositories;
using AutoBackend.Sdk.Data.Storage;
using AutoBackend.Sdk.Filters;

namespace AutoBackend.Sdk.GraphQL;

internal class GenericGqlQuery<TEntity, TFilter>
    where TEntity : class
    where TFilter : class, IGenericFilter
{
    [GraphQLName("list")]
    [UseProjection]
    public IQueryable<TEntity> GetList(
        [Service(ServiceKind.Resolver)] IGenericStorage<TEntity, TFilter> genericStorage,
        [GraphQLName("filter")] TFilter? filter)
    {
        return genericStorage.GetQuery(filter);
    }

    [GraphQLName("count")]
    public Task<long> GetCountAsync(
        [Service(ServiceKind.Resolver)] IGenericRepository<TEntity, TFilter> genericRepository,
        [GraphQLName("filter")] TFilter? filter)
    {
        return genericRepository.GetCountAsync(filter);
    }
}

internal class GenericGqlMutation<TEntity>
    where TEntity : class
{
}