using AutoBackend.Sdk.Models;

namespace AutoBackend.Sdk.Data.Mappers;

internal interface IGenericRequestMapper
{
    TEntity ToEntity<TEntity, TModel>(TModel model)
        where TEntity : class, new()
        where TModel : class, IGenericRequest;
}