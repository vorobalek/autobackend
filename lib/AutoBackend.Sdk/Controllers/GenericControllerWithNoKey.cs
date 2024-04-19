using AutoBackend.Sdk.Data.Mappers;
using AutoBackend.Sdk.Data.Repositories;
using AutoBackend.Sdk.Filters;
using AutoBackend.Sdk.Models;
using AutoBackend.Sdk.Services.CancellationTokenProvider;
using Microsoft.AspNetCore.Mvc;

namespace AutoBackend.Sdk.Controllers;

internal sealed class GenericControllerWithNoKey<
    TEntity,
    TRequest,
    TResponse,
    TFilter
>(
    IGenericRequestMapper<TEntity, TRequest> genericRequestMapper,
    IGenericResponseMapper<TEntity, TResponse> genericResponseMapper,
    IGenericRepositoryWithNoKey<TEntity, TFilter> genericRepository,
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
{
    private readonly ICancellationTokenProvider _cancellationTokenProvider = cancellationTokenProvider;
    private readonly IGenericResponseMapper<TEntity, TResponse> _genericResponseMapper = genericResponseMapper;

    [HttpPost]
    public Task<ActionResult<GenericControllerResponse<TResponse>>> CreateAsync(
        [FromBody] TRequest request)
    {
        return ProcessAsync(
            async cancellationToken =>
                _genericResponseMapper
                    .ToModel(
                        await genericRepository
                            .CreateAsync(
                                genericRequestMapper.ToEntity(request),
                                cancellationToken)),
            _cancellationTokenProvider.GlobalCancellationToken);
    }
}