namespace AutoBackend.Sdk.Exceptions.Api;

internal abstract class ApiException : Exception
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

    public abstract int StatusCode { get; }
}