using System.Text.Json.Serialization;
using AutoBackend.Sdk.Extensions;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace AutoBackend.Sdk.Models;

internal record GenericControllerResponse(
    [property: JsonProperty("ok", Order = 0)]
    [property: JsonPropertyName("ok")]
    bool Ok,
    [property: JsonProperty("error_code", Order = 2)]
    [property: JsonPropertyName("error_code")]
    int? ErrorCode = null,
    [property: JsonProperty("description", Order = 3)]
    [property: JsonPropertyName("description")]
    string? Description = null)
{
    [JsonProperty("request_time_ms", Order = 4)]
    [JsonPropertyName("request_time_ms")]
    public double? RequestTimeMs { get; private set; }

    internal GenericControllerResponse WithRequestTime(HttpContext httpContext)
    {
        RequestTimeMs = httpContext.TryGetRequestTimeMs();

        return this;
    }
}

internal sealed record GenericControllerResponse<T>(
    bool Ok,
    [property: JsonProperty("result", Order = 1)]
    [property: JsonPropertyName("result")]
    T? Result = default,
    int? ErrorCode = null,
    string? Description = null)
    : GenericControllerResponse(Ok, ErrorCode, Description)
{
    internal new GenericControllerResponse<T> WithRequestTime(HttpContext httpContext)
    {
        return (base.WithRequestTime(httpContext) as GenericControllerResponse<T>)!;
    }
}