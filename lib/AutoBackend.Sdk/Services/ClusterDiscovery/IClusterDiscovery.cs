using Microsoft.AspNetCore.Http;

namespace AutoBackend.Sdk.Services.ClusterDiscovery;

internal interface IClusterDiscovery
{
    ClusterNode CurrentClusterNode { get; }

    Task ProcessDiscoveryRequest(
        HttpContext httpContext,
        ClusterNode? remoteClusterNode = null,
        CancellationToken cancellationToken = default);

    Task Discover(CancellationToken cancellationToken);
}