using AutoBackend.Sdk.Filters;
using AutoBackend.Sdk.Models;

namespace AutoBackend.Sdk.GraphQL.Queries;

internal abstract class GenericGqlQueryWithNoKey<
    TEntity,
    TResponse,
    TFilter
> : GenericGqlQuery<
    TEntity,
    TResponse,
    TFilter
>
    where TEntity : class, new()
    where TResponse : class, IGenericResponse, new()
    where TFilter : class, IGenericFilter;