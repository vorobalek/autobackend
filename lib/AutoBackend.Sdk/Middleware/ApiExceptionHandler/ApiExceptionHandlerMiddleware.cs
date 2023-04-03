using AutoBackend.Sdk.Extensions;
using AutoBackend.Sdk.Services.ExceptionHandler;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace AutoBackend.Sdk.Middleware.ApiExceptionHandler;

internal sealed class ApiExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public ApiExceptionHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(
        HttpContext httpContext,
        IExceptionHandlerFactory exceptionHandlerFactory,
        ILogger<ApiExceptionHandlerMiddleware> logger)
    {
        exceptionHandlerFactory.SetCurrentHandler(
            httpContext,
            HandleExceptionStatusCodeDefaultAsync,
            HandleExceptionResponseDefaultAsyncProvider(logger));
        try
        {
            await _next(httpContext);
        }
        catch (Exception exception)
        {
            await exceptionHandlerFactory.CurrentHandler!.HandleAsync(exception, CancellationToken.None);
        }
    }

    private Task<int> HandleExceptionStatusCodeDefaultAsync(Exception exception)
    {
        return Task.FromResult(exception.ToApiException().StatusCode);
    }

    private Func<Exception, Task<string>> HandleExceptionResponseDefaultAsyncProvider(ILogger logger)
    {
        Task<string> HandleExceptionResponseDefaultAsync(Exception exception)
        {
            logger.LogCritical(
                exception,
                Constants.AnUnexpectedInternalServerErrorHasHappenedOutOfTheControllerContext);
            return Task.FromResult(Constants.AnUnexpectedInternalServerErrorHasHappened);
        }

        return HandleExceptionResponseDefaultAsync;
    }
}