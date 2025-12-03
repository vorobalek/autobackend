namespace AutoBackend.Sdk.Exceptions.Data;

internal class InconsistentDataException : DataException
{
    internal InconsistentDataException()
    {
    }

    internal InconsistentDataException(string message) : base(message)
    {
    }
}