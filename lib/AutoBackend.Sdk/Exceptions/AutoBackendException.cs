namespace AutoBackend.Sdk.Exceptions;

public abstract class AutoBackendException : Exception
{
    protected AutoBackendException()
    {
    }

    protected AutoBackendException(string? message) : base(message)
    {
    }

    protected AutoBackendException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}