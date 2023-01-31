using Microsoft.AspNetCore.Builder;

namespace AutoBackend.Sdk.Middleware.ApiExceptionHandler;

internal static class ApiExceptionHandlerMiddlewareExtensions
{
    public static IApplicationBuilder UseApiExceptionHandler(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ApiExceptionHandlerMiddleware>();
    }
}