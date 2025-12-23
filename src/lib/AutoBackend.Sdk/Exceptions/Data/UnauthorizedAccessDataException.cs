namespace AutoBackend.Sdk.Exceptions.Data;

internal class UnauthorizedAccessDataException : DataException
{
    internal UnauthorizedAccessDataException(string message) : base(message)
    {
    }
    internal UnauthorizedAccessDataException(string message, Exception innerException) : base(message, innerException)
    {
    }
}