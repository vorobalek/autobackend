using Microsoft.AspNetCore.Http;

namespace AutoBackend.Sdk.Services.ExceptionHandler;

internal interface IExceptionHandlerFactory
{
    IExceptionHandler? CurrentHandler { get; }

    void SetCurrentHandler<TResponse>(
        HttpContext httpContext,
        Func<Exception, Task<int>> handleExceptionStatusCodeAsync,
        Func<Exception, Task<TResponse>> handleExceptionResponseAsync);
}