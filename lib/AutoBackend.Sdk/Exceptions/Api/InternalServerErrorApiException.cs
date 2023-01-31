using Microsoft.AspNetCore.Http;

namespace AutoBackend.Sdk.Exceptions.Api;

internal class InternalServerErrorApiException : ApiException
{
    public const string ErrorMessage = "Unexpected internal server error";

    public InternalServerErrorApiException()
    {
    }

    public InternalServerErrorApiException(string message) : base(message)
    {
    }

    public InternalServerErrorApiException(string message, Exception innerException) : base(message, innerException)
    {
    }

    public override int StatusCode => StatusCodes.Status500InternalServerError;
}