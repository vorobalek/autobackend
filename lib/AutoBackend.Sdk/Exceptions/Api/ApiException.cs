namespace AutoBackend.Sdk.Exceptions.Api;

internal abstract class ApiException : AutoBackendException
{
    protected ApiException()
    {
    }

    protected ApiException(string message) : base(message)
    {
    }

    protected ApiException(string message, Exception innerException) : base(message, innerException)
    {
    }

    internal abstract int StatusCode { get; }
}