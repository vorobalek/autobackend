using AutoBackend.Sdk.Storage;

namespace AutoBackend.Sdk.GraphQL;

internal class GenericGqlQuery<TEntity>
    where TEntity : class
{
    [GraphQLName("all")]
    public Task<TEntity[]> GetAllAsync(
        [Service] IGenericStorage<TEntity> genericStorage)
    {
        return genericStorage.GetAllAsync();
    }

    [GraphQLName("slice")]
    public Task<TEntity[]> GetSliceAsync(
        [GraphQLName("skip")] int? skipCount,
        [GraphQLName("take")] int? takeCount,
        [Service] IGenericStorage<TEntity> genericStorage)
    {
        return genericStorage.GetSliceAsync(skipCount, takeCount);
    }

    [GraphQLName("count")]
    public Task<long> GetCountAsync(
        [Service] IGenericStorage<TEntity> genericStorage)
    {
        return genericStorage.GetCountAsync();
    }
}