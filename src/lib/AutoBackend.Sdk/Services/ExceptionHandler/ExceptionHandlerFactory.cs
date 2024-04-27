using System.Net.Mime;
using Microsoft.AspNetCore.Http;

namespace AutoBackend.Sdk.Services.ExceptionHandler;

internal sealed class ExceptionHandlerFactory : IExceptionHandlerFactory
{
    public IExceptionHandler? CurrentHandler { get; private set; }

    public void SetCurrentHandler<TResponse>(
        HttpContext httpContext,
        Func<Exception, Task<int>> handleExceptionStatusCodeAsync,
        Func<Exception, Task<TResponse>> handleExceptionResponseAsync)
    {
        CurrentHandler = new ExceptionHandler<TResponse>(
            httpContext,
            handleExceptionStatusCodeAsync,
            handleExceptionResponseAsync);
    }

    private sealed class ExceptionHandler<TResponse>(
        HttpContext httpContext,
        Func<Exception, Task<int>> processStatusCode,
        Func<Exception, Task<TResponse>> processResponse)
        : IExceptionHandler
    {
        public async Task HandleAsync(Exception exception, CancellationToken cancellationToken)
        {
            var statusCode = await processStatusCode(exception);
            var response = await processResponse(exception);
            httpContext.Response.StatusCode = statusCode;
            httpContext.Response.ContentType = MediaTypeNames.Application.Json;
            await httpContext.Response.WriteAsJsonAsync(response, cancellationToken);
        }
    }
}