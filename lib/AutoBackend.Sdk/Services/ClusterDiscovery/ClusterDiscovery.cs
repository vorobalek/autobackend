using System.Collections.Concurrent;
using AutoBackend.Sdk.Extensions;
using AutoBackend.Sdk.Models;
using AutoBackend.Sdk.Services.DateTimeProvider;
using Flurl.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AutoBackend.Sdk.Services.ClusterDiscovery;

internal sealed class ClusterDiscovery(
    IDateTimeProvider dateTimeProvider,
    ILogger<ClusterDiscovery> logger)
    : IClusterDiscovery
{
    private static readonly string? ClusterUrl = Environment.GetEnvironmentVariable(Constants.HostEnvironmentVariable);
    private static ConcurrentDictionary<Guid, ClusterNode>? _knownClusterNodes;
    private static ClusterNode? _currentNode;

    private ConcurrentDictionary<Guid, ClusterNode> KnownClusterNodes =>
        _knownClusterNodes ??= new ConcurrentDictionary<Guid, ClusterNode>
        {
            [CurrentClusterNode.Id] = CurrentClusterNode
        };

    public ClusterNode CurrentClusterNode =>
        _currentNode ??= new ClusterNode(
            true,
            Guid.NewGuid(),
            dateTimeProvider.UtcNow());

    public async Task ProcessDiscoveryRequest(
        HttpContext httpContext,
        CancellationToken cancellationToken,
        ClusterNode? remoteClusterNode = null)
    {
        var utcNow = dateTimeProvider.UtcNow();

        if (remoteClusterNode?.Id == CurrentClusterNode.Id)
        {
            await httpContext.Response.CompleteAsync();
            return;
        }

        Log(remoteClusterNode);

        if (remoteClusterNode is not null)
        {
            KnownClusterNodes[remoteClusterNode.Id] = FillRemoteNode(remoteClusterNode);
            KnownClusterNodes[remoteClusterNode.Id].LastSeenUtc = dateTimeProvider.UtcNow();
            KnownClusterNodes[remoteClusterNode.Id].LastSeenIp.WithValueIfNotNull(
                httpContext.Request.Headers[Constants.XForwardedForHeaderName],
                dateTimeProvider);
        }

        CurrentClusterNode.LastRequestToUtc.WithValue(utcNow, dateTimeProvider);

        await httpContext.WriteJsonAndCompleteAsync(
            GenericControllerResponse.CreateOk(
                httpContext,
                KnownClusterNodes.Values.ToArray()),
            Formatting.None,
            cancellationToken);
    }

    public async Task Discover(CancellationToken cancellationToken)
    {
        if (Uri.TryCreate(ClusterUrl, UriKind.Absolute, out var clusterUri) &&
            Uri.TryCreate(clusterUri, Constants.ClusterDiscoveryServiceUrl, out var serviceUri))
        {
            var response = await serviceUri
                .PostJsonAsync(CurrentClusterNode, cancellationToken: cancellationToken)
                .ReceiveJson<GenericControllerResponse<ClusterNode[]?>?>();

            if (response is { Ok: true, Result: { } remoteClusterNodes })
                foreach (var remoteClusterNode in remoteClusterNodes)
                {
                    if (remoteClusterNode.Id != CurrentClusterNode.Id)
                    {
                        KnownClusterNodes[remoteClusterNode.Id] = FillRemoteNode(remoteClusterNode);
                    }
                    else
                    {
                        CurrentClusterNode
                            .LastRequestToUtc
                            .WithNewestValueIfNotNull(remoteClusterNode.LastRequestToUtc);
                        CurrentClusterNode
                            .LastRequestFromUtc
                            .WithNewestValueIfNotNull(remoteClusterNode.LastRequestFromUtc);
                        CurrentClusterNode
                            .LastRequestTimeMs
                            .WithNewestValueIfNotNull(remoteClusterNode.LastRequestTimeMs);
                        CurrentClusterNode
                            .LastSeenIp
                            .WithNewestValueIfNotNull(remoteClusterNode.LastSeenIp);
                        CurrentClusterNode.LastSeenUtc = remoteClusterNode.LastSeenUtc;
                    }

                    if (remoteClusterNode.IsCurrent)
                    {
                        KnownClusterNodes[remoteClusterNode.Id]
                            .LastRequestFromUtc
                            .WithValueIfNotNull(
                                dateTimeProvider.UtcNow(),
                                dateTimeProvider);
                        KnownClusterNodes[remoteClusterNode.Id]
                            .LastRequestTimeMs
                            .WithValueIfNotNull(
                                response.RequestTimeMs,
                                dateTimeProvider);
                        KnownClusterNodes[remoteClusterNode.Id].LastSeenUtc = dateTimeProvider.UtcNow();
                    }
                }
        }

        foreach (var knownKeyValue in KnownClusterNodes
                     .Where(knownKeyValue =>
                         ((dateTimeProvider.UtcNow() - knownKeyValue.Value.LastSeenUtc)?.TotalSeconds ??
                          double.MaxValue)
                         > 10
                         && knownKeyValue.Key != CurrentClusterNode.Id))
            KnownClusterNodes.Remove(knownKeyValue.Key, out _);
    }

    private ClusterNode FillRemoteNode(ClusterNode remoteClusterNode)
    {
        return new ClusterNode(
            false,
            remoteClusterNode.Id,
            remoteClusterNode.CreatedUtc)
        {
            LastSeenIp = FillRemoteNodeField(x => x.LastSeenIp, remoteClusterNode),
            LastRequestToUtc = FillRemoteNodeField(x => x.LastRequestToUtc, remoteClusterNode),
            LastRequestFromUtc = FillRemoteNodeField(x => x.LastRequestFromUtc, remoteClusterNode),
            LastRequestTimeMs = FillRemoteNodeField(x => x.LastRequestTimeMs, remoteClusterNode)
        };
    }

    private ClusterNodeField<T> FillRemoteNodeField<T>(
        Func<ClusterNode, ClusterNodeField<T>> selector,
        ClusterNode remoteClusterNode)
    {
        if (selector is null) throw new ArgumentNullException(nameof(selector));

        var existedClusterNode = KnownClusterNodes.GetValueOrDefault(remoteClusterNode.Id);

        var newClusterNodeField = selector(remoteClusterNode);
        if (existedClusterNode is not null && selector(existedClusterNode) is { } existedClusterNodeField)
            return existedClusterNodeField.WithNewestValueIfNotNull(newClusterNodeField);

        return newClusterNodeField;
    }

    private void Log(ClusterNode? remoteClusterNode)
    {
        logger.LogDebug(
            new EventId(
                42,
                "discovery request"),
            Constants.ClusterDiscoveryRequestLogMessage,
            CurrentClusterNode.Id,
            CurrentClusterNode.CreatedUtc,
            CurrentClusterNode.LastRequestToUtc.Value,
            CurrentClusterNode.LastRequestFromUtc.Value,
            CurrentClusterNode.LastRequestTimeMs.Value,
            remoteClusterNode?.Id,
            remoteClusterNode?.CreatedUtc,
            remoteClusterNode?.LastRequestToUtc.Value,
            remoteClusterNode?.LastRequestFromUtc.Value,
            remoteClusterNode?.LastRequestTimeMs.Value);
    }
}