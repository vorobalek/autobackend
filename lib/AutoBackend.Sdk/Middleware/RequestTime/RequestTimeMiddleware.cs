using AutoBackend.Sdk.Services.ClusterDiscovery;
using AutoBackend.Sdk.Services.DateTimeProvider;
using Microsoft.AspNetCore.Http;

namespace AutoBackend.Sdk.Middleware.RequestTime;

internal sealed class RequestTimeMiddleware(RequestDelegate next)
{
    public async Task Invoke(
        HttpContext httpContext,
        IDateTimeProvider dateTimeProvider,
        IClusterDiscovery clusterDiscovery)
    {
        var requestStartedUtc = dateTimeProvider.UtcNow();
        httpContext.Items.Add(Constants.RequestStartedOnContextItemName, requestStartedUtc);
        await next(httpContext);
        clusterDiscovery.CurrentClusterNode
            .LastRequestTimeMs
            .WithValueIfNotNull(
                (dateTimeProvider.UtcNow() - requestStartedUtc).TotalMilliseconds,
                dateTimeProvider);
    }
}