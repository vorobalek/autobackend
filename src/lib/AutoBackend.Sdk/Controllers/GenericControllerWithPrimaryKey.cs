using AutoBackend.Sdk.Data.Mappers;
using AutoBackend.Sdk.Data.Repositories;
using AutoBackend.Sdk.Filters;
using AutoBackend.Sdk.Models;
using AutoBackend.Sdk.Services.CancellationTokenProvider;
using Microsoft.AspNetCore.Mvc;

namespace AutoBackend.Sdk.Controllers;

internal sealed class GenericControllerWithPrimaryKey<
    TEntity,
    TRequest,
    TResponse,
    TFilter,
    TKey
>(
    IGenericRequestMapper genericRequestMapper,
    IGenericResponseMapper genericResponseMapper,
    IGenericRepositoryWithPrimaryKey<
        TEntity,
        TFilter,
        TKey
    > genericRepository,
    ICancellationTokenProvider cancellationTokenProvider)
    : GenericController<
        TEntity,
        TResponse,
        TFilter
    >(
        genericResponseMapper,
        genericRepository,
        cancellationTokenProvider)
    where TEntity : class, new()
    where TRequest : class, IGenericRequest
    where TResponse : class, IGenericResponse, new()
    where TFilter : class, IGenericFilter
    where TKey : notnull
{
    private readonly ICancellationTokenProvider _cancellationTokenProvider = cancellationTokenProvider;
    private readonly IGenericResponseMapper _genericResponseMapper = genericResponseMapper;

    [HttpGet("{key}")]
    public Task<ActionResult<GenericControllerResponse<TResponse?>>> GetByPrimaryKeyAsync(
        [FromRoute] TKey key)
    {
        return ProcessAsync(async cancellationToken =>
                await genericRepository
                    .GetByPrimaryKeyAsync(
                        key,
                        cancellationToken) is { } entity
                    ? _genericResponseMapper.ToModel<TEntity, TResponse>(entity)
                    : null,
            _cancellationTokenProvider.GlobalToken);
    }

    [HttpPost("{key}")]
    public Task<ActionResult<GenericControllerResponse<TResponse>>> CreateByPrimaryKeyAsync(
        [FromRoute] TKey key,
        [FromBody] TRequest request)
    {
        return ProcessAsync(async cancellationToken =>
                _genericResponseMapper
                    .ToModel<TEntity, TResponse>(
                        await genericRepository
                            .CreateByPrimaryKeyAsync(
                                key,
                                genericRequestMapper.ToEntity<TEntity, TRequest>(request),
                                cancellationToken)),
            _cancellationTokenProvider.GlobalToken);
    }

    [HttpPut("{key}")]
    public Task<ActionResult<GenericControllerResponse<TResponse>>> UpdateByPrimaryKeyAsync(
        [FromRoute] TKey key,
        [FromBody] TRequest request)
    {
        return ProcessAsync(async cancellationToken =>
                _genericResponseMapper
                    .ToModel<TEntity, TResponse>(
                        await genericRepository
                            .UpdateByPrimaryKeyAsync(
                                key,
                                genericRequestMapper.ToEntity<TEntity, TRequest>(request),
                                cancellationToken)),
            _cancellationTokenProvider.GlobalToken);
    }

    [HttpDelete("{key}")]
    public Task<ActionResult<GenericControllerResponse>> DeleteByPrimaryKeyAsync(
        [FromRoute] TKey key)
    {
        return ProcessAsync(cancellationToken =>
                genericRepository
                    .DeleteByPrimaryKeyAsync(
                        key,
                        cancellationToken),
            _cancellationTokenProvider.GlobalToken);
    }
}