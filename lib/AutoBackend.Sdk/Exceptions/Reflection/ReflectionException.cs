namespace AutoBackend.Sdk.Exceptions.Reflection;

internal abstract class ReflectionException : AutoBackendException
{
    protected ReflectionException()
    {
    }

    protected ReflectionException(string message) : base(message)
    {
    }

    protected ReflectionException(string message, Exception innerException) : base(message, innerException)
    {
    }
}