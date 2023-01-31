namespace AutoBackend.Sdk.Exceptions;

public class AutoBackendException : Exception
{
    public AutoBackendException()
    {
    }

    public AutoBackendException(string? message) : base(message)
    {
    }

    public AutoBackendException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}