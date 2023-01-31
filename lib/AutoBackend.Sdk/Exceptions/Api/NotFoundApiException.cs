using Microsoft.AspNetCore.Http;

namespace AutoBackend.Sdk.Exceptions.Api;

internal class NotFoundApiException : ApiException
{
    public NotFoundApiException()
    {
    }

    public NotFoundApiException(string message) : base(message)
    {
    }

    public NotFoundApiException(string message, Exception innerException) : base(message, innerException)
    {
    }

    public override int StatusCode => StatusCodes.Status404NotFound;
}