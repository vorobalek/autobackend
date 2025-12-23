using Microsoft.AspNetCore.Http;

namespace AutoBackend.Sdk.Exceptions.Api;

internal sealed class UnauthorizedApiException : ApiException
{
    internal UnauthorizedApiException(string message, Exception innerException) : base(message, innerException)
    {
    }

    internal override int StatusCode => StatusCodes.Status401Unauthorized;
}