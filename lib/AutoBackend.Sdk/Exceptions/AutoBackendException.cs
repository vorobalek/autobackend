namespace AutoBackend.Sdk.Exceptions;

public class AutoBackendException : Exception
{
    internal AutoBackendException()
    {
    }

    internal AutoBackendException(string? message) : base(message)
    {
    }

    internal AutoBackendException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}