using System.Net.Mime;
using AutoBackend.Sdk.Services.DateTimeProvider;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace AutoBackend.Sdk.Extensions;

internal static class HttpContextExtensions
{
    internal static DateTimeOffset? TryGetRequestStartedDateTimeOffset(this HttpContext httpContext)
    {
        if (httpContext.Items.ContainsKey(Constants.RequestStartedOnContextItemName))
            if (httpContext.Items[Constants.RequestStartedOnContextItemName] is DateTimeOffset stamp)
                return stamp;

        return null;
    }

    internal static double? TryGetRequestTimeMs(this HttpContext httpContext)
    {
        if (httpContext.TryGetRequestStartedDateTimeOffset() is not { } stamp) return null;

        var dateTimeProvider = httpContext.RequestServices.GetRequiredService<IDateTimeProvider>();
        return (dateTimeProvider.UtcNow() - stamp).TotalMilliseconds;
    }

    internal static Task WriteJsonAsync(
        this HttpContext httpContext,
        object? value,
        Formatting formatting,
        CancellationToken cancellationToken)
    {
        var response = JsonConvert.SerializeObject(value, formatting);
        httpContext.Response.ContentType = MediaTypeNames.Application.Json;
        return httpContext.Response.WriteAsync(response, cancellationToken);
    }

    internal static async Task WriteJsonAndCompleteAsync(
        this HttpContext httpContext,
        object? value,
        Formatting formatting,
        CancellationToken cancellationToken)
    {
        await httpContext.WriteJsonAsync(value, formatting, cancellationToken);
        await httpContext.Response.CompleteAsync();
    }
}