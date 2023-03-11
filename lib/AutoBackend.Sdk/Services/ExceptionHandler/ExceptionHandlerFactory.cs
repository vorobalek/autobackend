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

    private sealed class ExceptionHandler<TResponse> : IExceptionHandler
    {
        private readonly HttpContext _httpContext;
        private readonly Func<Exception, Task<TResponse>> _processResponse;
        private readonly Func<Exception, Task<int>> _processStatusCode;

        public ExceptionHandler(
            HttpContext httpContext,
            Func<Exception, Task<int>> processStatusCode,
            Func<Exception, Task<TResponse>> processResponse)
        {
            _httpContext = httpContext;
            _processResponse = processResponse;
            _processStatusCode = processStatusCode;
        }

        public async Task HandleAsync(Exception exception, CancellationToken cancellationToken)
        {
            var statusCode = await _processStatusCode(exception);
            var response = await _processResponse(exception);
            _httpContext.Response.StatusCode = statusCode;
            _httpContext.Response.ContentType = MediaTypeNames.Application.Json;
            await _httpContext.Response.WriteAsJsonAsync(response, cancellationToken);
        }
    }
}