using AutoBackend.Sdk.Data.Mappers;
using AutoBackend.Sdk.Data.Repositories;
using AutoBackend.Sdk.Filters;
using AutoBackend.Sdk.Models;
using AutoBackend.Sdk.Services.CancellationTokenProvider;
using Microsoft.AspNetCore.Mvc;

namespace AutoBackend.Sdk.Controllers;

internal sealed class GenericControllerWithComplexKey<
    TEntity,
    TRequest,
    TResponse,
    TFilter,
    TKey1,
    TKey2
>(
    IGenericRequestMapper genericRequestMapper,
    IGenericResponseMapper genericResponseMapper,
    IGenericRepositoryWithComplexKey<
        TEntity,
        TFilter,
        TKey1,
        TKey2
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
    where TKey1 : notnull
    where TKey2 : notnull
{
    private readonly ICancellationTokenProvider _cancellationTokenProvider = cancellationTokenProvider;
    private readonly IGenericResponseMapper _genericResponseMapper = genericResponseMapper;

    [HttpGet("{key1}/{key2}")]
    public Task<ActionResult<GenericControllerResponse<TResponse?>>> GetByComplexKeyAsync(
        [FromRoute] TKey1 key1,
        [FromRoute] TKey2 key2)
    {
        return ProcessAsync(async cancellationToken =>
                await genericRepository
                    .GetByComplexKeyAsync(
                        key1,
                        key2,
                        cancellationToken) is { } entity
                    ? _genericResponseMapper.ToModel<TEntity, TResponse>(entity)
                    : null,
            _cancellationTokenProvider.GlobalCancellationToken);
    }

    [HttpPost("{key1}/{key2}")]
    public Task<ActionResult<GenericControllerResponse<TResponse>>> CreateByComplexKeyAsync(
        [FromRoute] TKey1 key1,
        [FromRoute] TKey2 key2,
        [FromBody] TRequest request)
    {
        return ProcessAsync(async cancellationToken =>
                _genericResponseMapper
                    .ToModel<TEntity, TResponse>(
                        await genericRepository
                            .CreateByComplexKeyAsync(
                                key1,
                                key2,
                                genericRequestMapper.ToEntity<TEntity, TRequest>(request),
                                cancellationToken)),
            _cancellationTokenProvider.GlobalCancellationToken);
    }

    [HttpPut("{key1}/{key2}")]
    public Task<ActionResult<GenericControllerResponse<TResponse>>> UpdateByComplexKeyAsync(
        [FromRoute] TKey1 key1,
        [FromRoute] TKey2 key2,
        [FromBody] TRequest request)
    {
        return ProcessAsync(async cancellationToken =>
                _genericResponseMapper
                    .ToModel<TEntity, TResponse>(
                        await genericRepository
                            .UpdateByComplexKeyAsync(
                                key1,
                                key2,
                                genericRequestMapper.ToEntity<TEntity, TRequest>(request),
                                cancellationToken)),
            _cancellationTokenProvider.GlobalCancellationToken);
    }

    [HttpDelete("{key1}/{key2}")]
    public Task<ActionResult<GenericControllerResponse>> DeleteByComplexKeyAsync(
        [FromRoute] TKey1 key1,
        [FromRoute] TKey2 key2)
    {
        return ProcessAsync(cancellationToken =>
                genericRepository
                    .DeleteByComplexKeyAsync(
                        key1,
                        key2,
                        cancellationToken),
            _cancellationTokenProvider.GlobalCancellationToken);
    }
}