using AutoBackend.Sdk.Filters;
using AutoBackend.Sdk.Models;

namespace AutoBackend.Sdk.GraphQL.Mutations;

internal abstract class GenericGqlMutation<
    TEntity,
    TRequest,
    TResponse,
    TFilter
>
    where TEntity : class, new()
    where TRequest : class, IGenericRequest
    where TResponse : class, IGenericResponse, new()
    where TFilter : class, IGenericFilter;