namespace AutoBackend.Sdk.Exceptions.Data;

internal class NotFoundDataException : DataException
{
    internal NotFoundDataException(string message) : base(message)
    {
    }
}