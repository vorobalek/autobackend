using Microsoft.AspNetCore.Http;

namespace AutoBackend.Sdk.Exceptions.Api;

internal sealed class NotFoundApiException : ApiException
{
    internal NotFoundApiException()
    {
    }

    internal NotFoundApiException(string message) : base(message)
    {
    }

    internal NotFoundApiException(string message, Exception innerException) : base(message, innerException)
    {
    }

    internal override int StatusCode => StatusCodes.Status404NotFound;
}