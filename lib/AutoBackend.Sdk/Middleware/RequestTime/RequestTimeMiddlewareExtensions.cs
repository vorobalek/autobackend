using Microsoft.AspNetCore.Builder;

namespace AutoBackend.Sdk.Middleware.RequestTime;

internal static class RequestTimeMiddlewareExtensions
{
    public static IApplicationBuilder UseRequestTimestamp(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<RequestTimeMiddleware>();
    }
}