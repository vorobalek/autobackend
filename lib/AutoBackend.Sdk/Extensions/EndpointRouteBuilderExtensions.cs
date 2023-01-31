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
        builder.MapGet(ClusterDiscovery.ServiceUrl, async context =>
        {
            var discoveryService = context.RequestServices.GetRequiredService<IClusterDiscovery>();
            await discoveryService.ProcessDiscoveryRequest(context);
        });
        builder.MapPost(ClusterDiscovery.ServiceUrl, async (
            HttpContext context,
            [FromBody] ClusterNode? node) =>
        {
            var discoveryService = context.RequestServices.GetRequiredService<IClusterDiscovery>();
            await discoveryService.ProcessDiscoveryRequest(context, node);
        });
        return builder;
    }
}