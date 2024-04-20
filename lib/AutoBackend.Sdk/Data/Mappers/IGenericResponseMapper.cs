using AutoBackend.Sdk.Models;

namespace AutoBackend.Sdk.Data.Mappers;

internal interface IGenericResponseMapper
{
    TResponse? ToModel<TEntity, TResponse>(TEntity? entity)
        where TEntity : class
        where TResponse : class, IGenericResponse, new();

    IEnumerable<TResponse>? ToModelEnumerable<TEntity, TResponse>(IEnumerable<TEntity>? entities)
        where TEntity : class
        where TResponse : class, IGenericResponse, new();
}