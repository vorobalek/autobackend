using Microsoft.AspNetCore.Builder;

namespace AutoBackend.Sdk.Middleware.RequestTime;

internal static class RequestTimeMiddlewareExtensions
{
    internal static IApplicationBuilder UseRequestTimestamp(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<RequestTimeMiddleware>();
    }
}