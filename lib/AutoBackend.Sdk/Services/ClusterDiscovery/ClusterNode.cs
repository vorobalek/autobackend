using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace AutoBackend.Sdk.Services.ClusterDiscovery;

internal sealed record ClusterNode(
    [property: JsonProperty("is_current", Order = 0)]
    [property: JsonPropertyName("is_current")]
    bool IsCurrent,
    [property: JsonProperty("id", Order = 1)]
    [property: JsonPropertyName("id")]
    Guid Id,
    [property: JsonProperty("created_utc", Order = 2)]
    [property: JsonPropertyName("created_utc")]
    DateTimeOffset CreatedUtc)
{
    [JsonProperty("last_seen_utc", Order = 3)]
    [JsonPropertyName("last_seen_utc")]
    internal DateTimeOffset? LastSeenUtc { get; set; }

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