using System.Text.Json.Serialization;
using AutoBackend.Sdk.Services.DateTimeProvider;
using Newtonsoft.Json;

namespace AutoBackend.Sdk.Services.ClusterDiscovery;

internal sealed record ClusterNodeField<T>
{
    [JsonProperty("value")]
    [JsonPropertyName("value")]
    internal T? Value { get; private set; }

    [JsonProperty("updated_utc")]
    [JsonPropertyName("updated_utc")]
    internal DateTimeOffset UpdatedUtc { get; private set; }

    internal ClusterNodeField<T> WithValue(T value, IDateTimeProvider dateTimeProvider)
    {
        Value = value;
        UpdatedUtc = dateTimeProvider.UtcNow();
        return this;
    }

    internal ClusterNodeField<T> WithValueIfNotNull(T? value, IDateTimeProvider dateTimeProvider)
    {
        return value is not null
            ? WithValue(value, dateTimeProvider)
            : this;
    }

    internal ClusterNodeField<T> WithNewestValueIfNotNull(ClusterNodeField<T> newest)
    {
        if (newest.UpdatedUtc > UpdatedUtc && newest.Value is not null)
        {
            Value = newest.Value;
            UpdatedUtc = newest.UpdatedUtc;
        }

        return this;
    }
}