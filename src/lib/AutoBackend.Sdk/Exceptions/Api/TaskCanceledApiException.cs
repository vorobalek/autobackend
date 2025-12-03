using Microsoft.AspNetCore.Http;

namespace AutoBackend.Sdk.Exceptions.Api;

internal sealed class TaskCanceledApiException : ApiException
{
    internal TaskCanceledApiException(string message, Exception innerException) : base(message, innerException)
    {
    }

    internal override int StatusCode => StatusCodes.Status408RequestTimeout;
}