using Microsoft.AspNetCore.Http;

namespace AutoBackend.Sdk.Exceptions.Api;

internal sealed class BadRequestApiException : ApiException
{
    internal BadRequestApiException()
    {
    }

    internal BadRequestApiException(string message) : base(message)
    {
    }

    internal BadRequestApiException(string message, Exception innerException) : base(message, innerException)
    {
    }

    internal override int StatusCode => StatusCodes.Status400BadRequest;
}