using Microsoft.AspNetCore.Http;

namespace AutoBackend.Sdk.Services.ExceptionHandler;

internal interface IExceptionHandlerFactory
{
    IApiExceptionHandler? CurrentApiExceptionHandler { get; }
    
    void SetCurrentApiHandler<TResponse>(
        HttpContext httpContext,
        Func<Exception, int> handleExceptionStatusCode,
        Func<Exception, TResponse> handleExceptionResponse);
}