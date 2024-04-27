using AutoBackend.Sdk.Services.CancellationTokenProvider;
using AutoBackend.Sdk.Services.ClusterDiscovery;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace AutoBackend.Sdk.Extensions;

internal static class EndpointRouteBuilderExtensions
{
    internal static IEndpointRouteBuilder MapClusterDiscovery(this IEndpointRouteBuilder builder)
    {
        builder.MapGet(Constants.ClusterDiscoveryServiceUrl, async context =>
        {
            var cancellationTokenProvider = context.RequestServices.GetRequiredService<ICancellationTokenProvider>();
            var discoveryService = context.RequestServices.GetRequiredService<IClusterDiscovery>();
            await discoveryService.ProcessDiscoveryRequest(
                context,
                cancellationTokenProvider.GlobalToken);
        });
        builder.MapPost(Constants.ClusterDiscoveryServiceUrl, async (
            HttpContext context,
            [FromBody] ClusterNode? node) =>
        {
            var cancellationTokenProvider = context.RequestServices.GetRequiredService<ICancellationTokenProvider>();
            var discoveryService = context.RequestServices.GetRequiredService<IClusterDiscovery>();
            await discoveryService.ProcessDiscoveryRequest(
                context,
                cancellationTokenProvider.GlobalToken,
                node);
        });
        return builder;
    }
}