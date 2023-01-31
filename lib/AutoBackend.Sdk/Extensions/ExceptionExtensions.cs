using AutoBackend.Sdk.Exceptions.Api;
using Microsoft.AspNetCore.Http;

namespace AutoBackend.Sdk.Extensions;

internal static class ExceptionExtensions
{
    public static int StatusCode(this Exception exception)
    {
        return exception switch
        {
            ApiException apiException => apiException.StatusCode,
            TaskCanceledException => StatusCodes.Status408RequestTimeout,
            _ => StatusCodes.Status500InternalServerError
        };
    }
}