using AutoBackend.Sdk.Extensions;
using AutoBackend.Sdk.Services.ExceptionHandler;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace AutoBackend.Sdk.Middleware.ApiExceptionHandler;

internal sealed class ApiExceptionHandlerMiddleware(RequestDelegate next)
{
    public async Task Invoke(
        HttpContext httpContext,
        IExceptionHandlerFactory exceptionHandlerFactory,
        ILogger<ApiExceptionHandlerMiddleware> logger)
    {
        exceptionHandlerFactory.SetCurrentApiHandler(
            httpContext,
            HandleExceptionStatusCodeDefault,
            HandleExceptionResponseDefaultAsyncProvider(logger));
        try
        {
            await next(httpContext);
        }
        catch (Exception exception)
        {
            await exceptionHandlerFactory.CurrentApiExceptionHandler!.HandleAsync(exception, CancellationToken.None);
        }
    }

    private static int HandleExceptionStatusCodeDefault(Exception exception)
    {
        return exception.ToApiException().StatusCode;
    }

    private static Func<Exception, string> HandleExceptionResponseDefaultAsyncProvider(ILogger logger)
    {
        return HandleExceptionResponseDefaultAsync;

        string HandleExceptionResponseDefaultAsync(Exception exception)
        {
            logger.LogCritical(
                exception,
                Constants.AnUnexpectedInternalServerErrorHasHappenedOutOfTheControllerContext);
            return Constants.AnUnexpectedInternalServerErrorHasHappened;
        }
    }
}