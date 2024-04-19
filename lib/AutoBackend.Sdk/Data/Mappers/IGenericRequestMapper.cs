using AutoBackend.Sdk.Models;

namespace AutoBackend.Sdk.Data.Mappers;

internal interface IGenericRequestMapper<out TEntity, in TModel>
    where TEntity : class, new()
    where TModel : class, IGenericRequest
{
    TEntity ToEntity(TModel model);
}