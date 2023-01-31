using Microsoft.AspNetCore.Http;

namespace AutoBackend.Sdk.Exceptions.Api;

internal class BadRequestApiException : ApiException
{
    public BadRequestApiException()
    {
    }

    public BadRequestApiException(string message) : base(message)
    {
    }

    public BadRequestApiException(string message, Exception innerException) : base(message, innerException)
    {
    }

    public override int StatusCode => StatusCodes.Status400BadRequest;
}