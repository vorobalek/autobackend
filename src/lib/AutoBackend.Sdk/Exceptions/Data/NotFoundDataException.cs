namespace AutoBackend.Sdk.Exceptions.Data;

internal class NotFoundDataException : DataException
{
    internal NotFoundDataException()
    {
    }

    internal NotFoundDataException(string message) : base(message)
    {
    }

    internal NotFoundDataException(string message, Exception innerException) : base(message, innerException)
    {
    }
}