using AutoBackend.Sdk.Exceptions.Api;
using AutoBackend.Sdk.Extensions;
using AutoBackend.Sdk.Models;
using AutoBackend.Sdk.Services.ExceptionHandler;
using AutoBackend.Sdk.Storage;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace AutoBackend.Sdk.Controllers;

internal abstract class GenericController : ControllerBase
{
    internal const string Version = "v1";
}

internal class GenericController<
    TEntity,
    TFilter
> : GenericController
    where TEntity : class
    where TFilter : class
{
    private readonly IGenericStorage<TEntity> _genericStorage;

    public GenericController(IGenericStorage<TEntity> genericStorage)
    {
        _genericStorage = genericStorage;
    }

    [HttpGet]
    public Task<ActionResult<GenericControllerResponse<TEntity[]>>> GetAllAsync()
    {
        return ProcessAsync(cancellationToken => _genericStorage.GetAllAsync(cancellationToken));
    }

    [HttpGet("count")]
    public Task<ActionResult<GenericControllerResponse<int>>> CountAsync()
    {
        return ProcessAsync(cancellationToken => _genericStorage.CountByFilterAsync<TFilter>(null, cancellationToken));
    }

    [HttpPost("filter")]
    public Task<ActionResult<GenericControllerResponse<TEntity[]>>> GetByFilterAsync(
        [FromBody] TFilter filter)
    {
        return ProcessAsync(cancellationToken => _genericStorage.GetByFilterAsync(filter, cancellationToken));
    }

    [HttpPost("filter/count")]
    public Task<ActionResult<GenericControllerResponse<int>>> CountByFilterAsync(
        [FromBody] TFilter filter)
    {
        return ProcessAsync(cancellationToken => _genericStorage.CountByFilterAsync(filter, cancellationToken));
    }

    protected async Task<ActionResult<GenericControllerResponse>> ProcessAsync(Func<CancellationToken, Task> resultAsyncProcessor)
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