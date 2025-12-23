using System.Net.Mime;
using Microsoft.AspNetCore.Http;

namespace AutoBackend.Sdk.Services.ExceptionHandler;

internal sealed class ExceptionHandlerFactory : IExceptionHandlerFactory
{
    public IApiExceptionHandler? CurrentApiExceptionHandler { get; private set; }

    public void SetCurrentApiHandler<TResponse>(
        HttpContext httpContext,
        Func<Exception, int> handleExceptionStatusCode,
        Func<Exception, TResponse> handleExceptionResponse)
    {
        CurrentApiExceptionHandler = new ApiExceptionHandler<TResponse>(
            httpContext,
            handleExceptionStatusCode,
            handleExceptionResponse);
    }

    private sealed class ApiExceptionHandler<TResponse>(
        HttpContext httpContext,
        Func<Exception, int> processStatusCode,
        Func<Exception, TResponse> processResponse)
        : IApiExceptionHandler
    {
        public async Task HandleAsync(Exception exception, CancellationToken cancellationToken)
        {
            var statusCode = processStatusCode(exception);
            var response = processResponse(exception);
            httpContext.Response.StatusCode = statusCode;
            httpContext.Response.ContentType = MediaTypeNames.Application.Json;
            await httpContext.Response.WriteAsJsonAsync(response, cancellationToken);
        }
    }
}