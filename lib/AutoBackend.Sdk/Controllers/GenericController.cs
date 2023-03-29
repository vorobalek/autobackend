using AutoBackend.Sdk.Data.Repositories;
using AutoBackend.Sdk.Exceptions.Api;
using AutoBackend.Sdk.Extensions;
using AutoBackend.Sdk.Filters;
using AutoBackend.Sdk.Models;
using AutoBackend.Sdk.Services.ExceptionHandler;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace AutoBackend.Sdk.Controllers;

internal abstract class GenericController : ControllerBase
{
    internal const string Version = "v1";

    protected async Task<ActionResult<GenericControllerResponse>> ProcessAsync(
        Func<CancellationToken, Task> resultAsyncProcessor)
    {
        SetupExceptionHandler();
        await resultAsyncProcessor(new CancellationTokenSource(TimeSpan.FromSeconds(5)).Token);
        return Ok(new GenericControllerResponse(true).WithRequestTime(HttpContext));
    }

    protected async Task<ActionResult<GenericControllerResponse<TResult>>> ProcessAsync<TResult>(
        Func<CancellationToken, Task<TResult>> resultAsyncProcessor)
    {
        SetupExceptionHandler();
        var result = await resultAsyncProcessor(new CancellationTokenSource(TimeSpan.FromSeconds(5)).Token);
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
        return Task.FromResult(exception.StatusCode());
    }

    private Task<GenericControllerResponse> HandleExceptionResponseAsync(Exception exception)
    {
        var statusCode = exception.StatusCode();
        return Task.FromResult(
            new GenericControllerResponse(
                    false,
                    statusCode,
                    statusCode == StatusCodes.Status500InternalServerError
                        ? InternalServerErrorApiException.ErrorMessage
                        : exception.Message)
                .WithRequestTime(HttpContext));
    }
}

internal abstract class GenericController<
    TEntity,
    TFilter
> : GenericController
    where TEntity : class
    where TFilter : class, IGenericFilter
{
    private readonly IGenericRepository<TEntity, TFilter> _genericRepository;

    protected GenericController(IGenericRepository<TEntity, TFilter> genericRepository)
    {
        _genericRepository = genericRepository;
    }

    [HttpGet]
    public Task<ActionResult<GenericControllerResponse<TEntity[]>>> GetAllAsync(
        [FromQuery] TFilter filter)
    {
        return ProcessAsync(cancellationToken =>
            _genericRepository.GetAllAsync(filter, cancellationToken));
    }

    [HttpGet("count")]
    public Task<ActionResult<GenericControllerResponse<long>>> GetCountAsync(
        [FromQuery] TFilter filter)
    {
        return ProcessAsync(cancellationToken =>
            _genericRepository.GetCountAsync(filter, cancellationToken));
    }
}