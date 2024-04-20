using AutoBackend.Sdk.Models;

namespace AutoBackend.Sdk.Data.Mappers;

internal interface IGenericRequestMapper
{
    TEntity ToEntity<TEntity, TRequest>(TRequest model)
        where TEntity : class, new()
        where TRequest : class, IGenericRequest;
}