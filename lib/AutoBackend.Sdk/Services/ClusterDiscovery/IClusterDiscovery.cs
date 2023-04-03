using Microsoft.AspNetCore.Http;

namespace AutoBackend.Sdk.Services.ClusterDiscovery;

internal interface IClusterDiscovery
{
    ClusterNode CurrentClusterNode { get; }

    Task ProcessDiscoveryRequest(
        HttpContext httpContext,
        CancellationToken cancellationToken,
        ClusterNode? remoteClusterNode = null);

    Task Discover(CancellationToken cancellationToken);
}