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
    int? ErrorCode = default,
    [property: JsonProperty("description", Order = 3)]
    [property: JsonPropertyName("description")]
    string? Description = default)
{
    [JsonProperty("request_time_ms", Order = 4)]
    [JsonPropertyName("request_time_ms")]
    public double? RequestTimeMs { get; private set; }

    internal GenericControllerResponse WithRequestTime(HttpContext httpContext)
    {
        RequestTimeMs = httpContext.TryGetRequestTimeMs();

        return this;
    }

    internal static GenericControllerResponse<T> CreateOk<T>(T? value = default)
    {
        return new GenericControllerResponse<T>(true, value);
    }

    internal static GenericControllerResponse<T> CreateOk<T>(HttpContext context, T? value = default)
    {
        return CreateOk(value).WithRequestTime(context);
    }
}

internal sealed record GenericControllerResponse<T>(
        bool Ok,
        [property: JsonProperty("result", Order = 1)]
        [property: JsonPropertyName("result")]
        T? Result = default,
        int? ErrorCode = default,
        string? Description = default)
    : GenericControllerResponse(Ok, ErrorCode, Description)
{
    internal new GenericControllerResponse<T> WithRequestTime(HttpContext httpContext)
    {
        return (base.WithRequestTime(httpContext) as GenericControllerResponse<T>)!;
    }
}