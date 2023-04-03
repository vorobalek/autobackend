using AutoBackend.Sdk.Filters;

namespace AutoBackend.Sdk.GraphQL.Mutations;

internal abstract class GenericGqlMutation<
    TEntity,
    TFilter
>
    where TEntity : class
    where TFilter : class, IGenericFilter
{
}