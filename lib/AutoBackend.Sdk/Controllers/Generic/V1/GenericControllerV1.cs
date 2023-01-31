using AutoBackend.Sdk.Exceptions.Api;
using AutoBackend.Sdk.Extensions;
using AutoBackend.Sdk.Models.V1;
using AutoBackend.Sdk.Services.ExceptionHandler;
using AutoBackend.Sdk.Storage.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace AutoBackend.Sdk.Controllers.Generic.V1;

internal class GenericControllerV1<
    TEntity,
    TFilter
> : GenericController
    where TEntity : class
    where TFilter : class
{
    private readonly IGenericStorage<TEntity> _genericStorage;

    public GenericControllerV1(IGenericStorage<TEntity> genericStorage)
    {
        _genericStorage = genericStorage;
    }

    [HttpGet]
    public Task<ActionResult<ApiResponseV1<TEntity[]>>> GetAllAsync()
    {
        return ProcessAsync(cancellationToken => _genericStorage.GetAllAsync(cancellationToken));
    }

    [HttpGet("count")]
    public Task<ActionResult<ApiResponseV1<int>>> CountAsync()
    {
        return ProcessAsync(cancellationToken => _genericStorage.CountByFilterAsync<TFilter>(null, cancellationToken));
    }

    [HttpPost("filter")]
    public Task<ActionResult<ApiResponseV1<TEntity[]>>> GetByFilterAsync(
        [FromBody] TFilter filter)
    {
        return ProcessAsync(cancellationToken => _genericStorage.GetByFilterAsync(filter, cancellationToken));
    }

    [HttpPost("filter/count")]
    public Task<ActionResult<ApiResponseV1<int>>> CountByFilterAsync(
        [FromBody] TFilter filter)
    {
        return ProcessAsync(cancellationToken => _genericStorage.CountByFilterAsync(filter, cancellationToken));
    }

    protected async Task<ActionResult<ApiResponseV1>> ProcessAsync(Func<CancellationToken, Task> resultAsyncProcessor)
    {
        SetupExceptionHandler();
        await resultAsyncProcessor(new CancellationTokenSource(TimeSpan.FromSeconds(5)).Token);
        return Ok(new ApiResponseV1(true).WithRequestTime(HttpContext));
    }

    protected async Task<ActionResult<ApiResponseV1<TResult>>> ProcessAsync<TResult>(
        Func<CancellationToken, Task<TResult>> resultAsyncProcessor)
    {
        SetupExceptionHandler();
        var result = await resultAsyncProcessor(new CancellationTokenSource(TimeSpan.FromSeconds(5)).Token);
        return new ApiResponseV1<TResult>(true, result).WithRequestTime(HttpContext);
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

    private Task<ApiResponseV1> HandleExceptionResponseAsync(Exception exception)
    {
        var statusCode = exception.StatusCode();
        return Task.FromResult(
            new ApiResponseV1(
                    false,
                    statusCode,
                    statusCode == StatusCodes.Status500InternalServerError
                        ? InternalServerErrorApiException.ErrorMessage
                        : exception.Message)
                .WithRequestTime(HttpContext));
    }
}