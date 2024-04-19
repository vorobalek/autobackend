using AutoBackend.Sdk.Models;

namespace AutoBackend.Sdk.Data.Mappers;

internal interface IGenericResponseMapper<in TEntity, out TModel>
    where TEntity : class
    where TModel : class, IGenericResponse, new()
{
    TModel ToModel(TEntity entity);
    IEnumerable<TModel> ToModel(IEnumerable<TEntity> entities);
}