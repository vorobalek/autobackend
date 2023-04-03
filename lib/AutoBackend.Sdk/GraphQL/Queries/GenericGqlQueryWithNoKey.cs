using AutoBackend.Sdk.Filters;

namespace AutoBackend.Sdk.GraphQL.Queries;

internal abstract class GenericGqlQueryWithNoKey<
    TEntity,
    TFilter
> : GenericGqlQuery<
    TEntity,
    TFilter
>
    where TEntity : class
    where TFilter : class, IGenericFilter
{
}