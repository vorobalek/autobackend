using AutoBackend.Sdk.Storage;

namespace AutoBackend.Sdk.GraphQL;

internal class GenericGqlQueryWithFilter<TEntity, TFilter>
    where TEntity : class
    where TFilter : class
{
    [GraphQLName("all")]
    public Task<TEntity[]> GetAllByFilterAsync(
        [Service] IGenericStorageWithFilter<TEntity, TFilter> genericStorage,
        [GraphQLName("filter")] TFilter? filter)
    {
        return genericStorage.GetAllByFilterAsync(filter);
    }

    [GraphQLName("slice")]
    public Task<TEntity[]> GetSliceAsync(
        [Service] IGenericStorageWithFilter<TEntity, TFilter> genericStorage,
        [GraphQLName("skip")] int? skipCount,
        [GraphQLName("take")] int? takeCount,
        [GraphQLName("filter")] TFilter? filter)
    {
        return genericStorage.GetSliceByFilterAsync(filter, skipCount, takeCount);
    }

    [GraphQLName("count")]
    public Task<long> GetCountAsync(
        [Service] IGenericStorageWithFilter<TEntity, TFilter> genericStorage,
        [GraphQLName("filter")] TFilter? filter)
    {
        return genericStorage.GetCountByFilterAsync(filter);
    }
}