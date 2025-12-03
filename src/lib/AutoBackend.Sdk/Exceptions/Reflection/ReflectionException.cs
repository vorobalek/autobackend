namespace AutoBackend.Sdk.Exceptions.Reflection;

internal abstract class ReflectionException : AutoBackendException
{
    protected ReflectionException()
    {
    }

    protected ReflectionException(string message) : base(message)
    {
    }
}