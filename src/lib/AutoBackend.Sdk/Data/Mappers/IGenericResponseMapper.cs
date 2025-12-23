using AutoBackend.Sdk.Models;

namespace AutoBackend.Sdk.Data.Mappers;

internal interface IGenericResponseMapper
{
    TResponse ToModel<TEntity, TResponse>(TEntity entity)
        where TEntity : class, new()
        where TResponse : class, IGenericResponse, new();

    IEnumerable<TResponse> ToModel<TEntity, TResponse>(IEnumerable<TEntity> entities)
        where TEntity : class, new()
        where TResponse : class, IGenericResponse, new();
}