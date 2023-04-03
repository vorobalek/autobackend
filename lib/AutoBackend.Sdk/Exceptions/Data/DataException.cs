namespace AutoBackend.Sdk.Exceptions.Data;

internal abstract class DataException : AutoBackendException
{
    protected DataException()
    {
    }

    protected DataException(string message) : base(message)
    {
    }

    protected DataException(string message, Exception innerException) : base(message, innerException)
    {
    }
}