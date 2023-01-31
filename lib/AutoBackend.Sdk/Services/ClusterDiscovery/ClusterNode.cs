using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace AutoBackend.Sdk.Services.ClusterDiscovery;

internal record ClusterNode
{
    [JsonProperty("is_current", Order = 0)]
    [JsonPropertyName("is_current")]
    public bool IsCurrent { get; init; }

    [JsonProperty("id", Order = 1)]
    [JsonPropertyName("id")]
    public Guid Id { get; init; }

    [JsonProperty("created_utc", Order = 2)]
    [JsonPropertyName("created_utc")]
    public DateTimeOffset CreatedUtc { get; init; }

    [JsonProperty("last_seen_utc", Order = 3)]
    [JsonPropertyName("last_seen_utc")]
    public DateTimeOffset LastSeenUtc { get; set; }

    [JsonProperty("last_seen_ip", Order = 4)]
    [JsonPropertyName("last_seen_ip")]
    public ClusterNodeField<string?> LastSeenIp { get; init; } = new();

    [JsonProperty("last_request_to_utc", Order = 5)]
    [JsonPropertyName("last_request_to_utc")]
    public ClusterNodeField<DateTimeOffset?> LastRequestToUtc { get; init; } = new();

    [JsonProperty("last_request_from_utc", Order = 6)]
    [JsonPropertyName("last_request_from_utc")]
    public ClusterNodeField<DateTimeOffset?> LastRequestFromUtc { get; init; } = new();

    [JsonProperty("last_request_time_ms", Order = 7)]
    [JsonPropertyName("last_request_time_ms")]
    public ClusterNodeField<double?> LastRequestTimeMs { get; init; } = new();
}