using AutoBackend.Sdk.Services.ClusterDiscovery;
using AutoBackend.Sdk.Services.DateTimeProvider;
using Microsoft.AspNetCore.Http;

namespace AutoBackend.Sdk.Middleware.RequestTime;

internal class RequestTimeMiddleware
{
    public const string RequestStartedOnContextItem = "RequestStartedOn";

    private readonly RequestDelegate _next;

    public RequestTimeMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(
        HttpContext httpContext,
        IDateTimeProvider dateTimeProvider,
        IClusterDiscovery clusterDiscovery)
    {
        var requestStartedUtc = dateTimeProvider.UtcNow();
        httpContext.Items.Add(RequestStartedOnContextItem, requestStartedUtc);
        await _next(httpContext);
        clusterDiscovery.CurrentClusterNode
            .LastRequestTimeMs
            .WithValueIfNotNull(
                (dateTimeProvider.UtcNow() - requestStartedUtc).TotalMilliseconds,
                dateTimeProvider);
    }
}