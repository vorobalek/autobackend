using Microsoft.AspNetCore.Http;

namespace AutoBackend.Sdk.Exceptions.Api;

internal sealed class InternalServerErrorApiException : ApiException
{
    internal InternalServerErrorApiException()
    {
    }

    internal InternalServerErrorApiException(string message) : base(message)
    {
    }

    internal InternalServerErrorApiException(string message, Exception innerException) : base(message, innerException)
    {
    }

    internal override int StatusCode => StatusCodes.Status500InternalServerError;
}