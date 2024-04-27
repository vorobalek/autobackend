using AutoBackend.Sdk.Services.DateTimeProvider;
using Microsoft.AspNetCore.Http;

namespace AutoBackend.Sdk.Middleware.RequestTime;

internal sealed class RequestTimeMiddleware(RequestDelegate next)
{
    public async Task Invoke(
        HttpContext httpContext,
        IDateTimeProvider dateTimeProvider)
    {
        var requestStartedUtc = dateTimeProvider.UtcNow();
        httpContext.Items.Add(Constants.RequestStartedOnContextItemName, requestStartedUtc);
        await next(httpContext);
    }
}