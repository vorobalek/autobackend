using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AutoBackend.Sdk.Filters;

internal abstract class GenericFilter : IGenericFilter
{
    [JsonProperty("skipCount")]
    [JsonPropertyName("skipCount")]
    [GraphQLName("skipCount")]
    [BindProperty(Name = "skipCount")]
    public int? SkipCount { get; set; }

    [JsonProperty("takeCount")]
    [JsonPropertyName("takeCount")]
    [GraphQLName("takeCount")]
    [BindProperty(Name = "takeCount")]
    public int? TakeCount { get; set; }
}