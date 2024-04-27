using Microsoft.AspNetCore.Builder;

namespace AutoBackend.Sdk.Middleware.ApiExceptionHandler;

internal static class ApiExceptionHandlerMiddlewareExtensions
{
    internal static IApplicationBuilder UseApiExceptionHandler(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ApiExceptionHandlerMiddleware>();
    }
}