using System.Text.Json.Serialization;
using AutoBackend.Sdk.Services.DateTimeProvider;
using Newtonsoft.Json;

namespace AutoBackend.Sdk.Services.ClusterDiscovery;

internal record ClusterNodeField<T>
{
    [JsonProperty("value")]
    [JsonPropertyName("value")]
    public T? Value { get; private set; }

    [JsonProperty("updated_utc")]
    [JsonPropertyName("updated_utc")]
    public DateTimeOffset UpdatedUtc { get; private set; }

    public static ClusterNodeField<T> Create(T? value, IDateTimeProvider dateTimeProvider)
    {
        return new ClusterNodeField<T>
        {
            Value = value,
            UpdatedUtc = dateTimeProvider.UtcNow()
        };
    }

    public ClusterNodeField<T> WithValue(T? value, IDateTimeProvider dateTimeProvider)
    {
        Value = value;
        UpdatedUtc = dateTimeProvider.UtcNow();
        return this;
    }

    public ClusterNodeField<T> WithValueIfNotNull(T? value, IDateTimeProvider dateTimeProvider)
    {
        if (value != null)
            return WithValue(value, dateTimeProvider);
        return this;
    }

    public ClusterNodeField<T> WithNewestValueIfNotNull(ClusterNodeField<T> newest)
    {
        if (newest.UpdatedUtc > UpdatedUtc && newest.Value != null)
        {
            Value = newest.Value;
            UpdatedUtc = newest.UpdatedUtc;
        }

        return this;
    }
}