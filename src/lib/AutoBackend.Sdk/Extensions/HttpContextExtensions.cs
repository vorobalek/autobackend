using AutoBackend.Sdk.Services.DateTimeProvider;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace AutoBackend.Sdk.Extensions;

internal static class HttpContextExtensions
{
    extension(HttpContext httpContext)
    {
        private DateTimeOffset? TryGetRequestStartedDateTimeOffset()
        {
            if (!httpContext.Items.TryGetValue(Constants.RequestStartedOnContextItemName, out var requestStarted)) return null;
            if (requestStarted is DateTimeOffset stamp)
                return stamp;

            return null;
        }

        internal double? TryGetRequestTimeMs()
        {
            if (httpContext.TryGetRequestStartedDateTimeOffset() is not { } stamp) return null;

            var dateTimeProvider = httpContext.RequestServices.GetRequiredService<IDateTimeProvider>();
            return (dateTimeProvider.UtcNow() - stamp).TotalMilliseconds;
        }
    }
}