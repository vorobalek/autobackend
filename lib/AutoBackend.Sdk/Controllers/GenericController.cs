using AutoBackend.Sdk.Data.Mappers;
using AutoBackend.Sdk.Data.Repositories;
using AutoBackend.Sdk.Extensions;
using AutoBackend.Sdk.Filters;
using AutoBackend.Sdk.Models;
using AutoBackend.Sdk.Services.CancellationTokenProvider;
using AutoBackend.Sdk.Services.ExceptionHandler;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace AutoBackend.Sdk.Controllers;

internal abstract class GenericController : ControllerBase
{
    protected async Task<ActionResult<GenericControllerResponse>> ProcessAsync(
        Func<CancellationToken, Task> resultAsyncProcessor,
        CancellationToken cancellationToken)
    {
        SetupExceptionHandler();
        await resultAsyncProcessor(cancellationToken);
        return Ok(new GenericControllerResponse(true).WithRequestTime(HttpContext));
    }

    protected async Task<ActionResult<GenericControllerResponse<TResult>>> ProcessAsync<TResult>(
        Func<CancellationToken, Task<TResult>> resultAsyncProcessor,
        CancellationToken cancellationToken)
    {
        SetupExceptionHandler();
        var result = await resultAsyncProcessor(cancellationToken);
        return new GenericControllerResponse<TResult>(true, result).WithRequestTime(HttpContext);
    }

    private void SetupExceptionHandler()
    {
        HttpContext
            .RequestServices
            .GetRequiredService<IExceptionHandlerFactory>().SetCurrentHandler(
                HttpContext,
                HandleExceptionStatusCodeAsync,
                HandleExceptionResponseAsync);
    }

    private Task<int> HandleExceptionStatusCodeAsync(Exception exception)
    {
        return Task.FromResult(exception.ToApiException().StatusCode);
    }

    private Task<GenericControllerResponse> HandleExceptionResponseAsync(Exception exception)
    {
        var statusCode = exception.ToApiException().StatusCode;
        return Task.FromResult(
            new GenericControllerResponse(
                    false,
                    statusCode,
                    statusCode == StatusCodes.Status500InternalServerError
                        ? Constants.AnUnexpectedInternalServerErrorHasHappened
                        : exception.Message)
                .WithRequestTime(HttpContext));
    }
}

internal abstract class GenericController<
    TEntity,
    TResponse,
    TFilter
>(
    IGenericResponseMapper genericResponseMapper,
    IGenericRepository<TEntity, TFilter> genericRepository,
    ICancellationTokenProvider cancellationTokenProvider) : GenericController
    where TEntity : class
    where TResponse : class, IGenericResponse, new()
    where TFilter : class, IGenericFilter
{
    [HttpGet]
    public Task<ActionResult<GenericControllerResponse<TResponse[]>>> GetAllAsync(
        [FromQuery] TFilter filter)
    {
        return ProcessAsync(async cancellationToken =>
                genericResponseMapper
                    .ToModel<TEntity, TResponse>(
                        await genericRepository
                            .GetAllAsync(
                                filter,
                                cancellationToken))
                    .ToArray(),
            cancellationTokenProvider.GlobalCancellationToken);
    }

    [HttpGet("count")]
    public Task<ActionResult<GenericControllerResponse<long>>> GetCountAsync(
        [FromQuery] TFilter filter)
    {
        return ProcessAsync(cancellationToken =>
                genericRepository
                    .GetCountAsync(
                        filter,
                        cancellationToken),
            cancellationTokenProvider.GlobalCancellationToken);
    }
}