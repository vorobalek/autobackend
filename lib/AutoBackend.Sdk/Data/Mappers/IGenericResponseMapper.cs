using AutoBackend.Sdk.Models;

namespace AutoBackend.Sdk.Data.Mappers;

internal interface IGenericResponseMapper
{
    TModel? ToModel<TEntity, TModel>(TEntity? entity)
        where TEntity : class
        where TModel : class, IGenericResponse, new();

    IEnumerable<TModel>? ToModelEnumerable<TEntity, TModel>(IEnumerable<TEntity>? entities)
        where TEntity : class
        where TModel : class, IGenericResponse, new();
}