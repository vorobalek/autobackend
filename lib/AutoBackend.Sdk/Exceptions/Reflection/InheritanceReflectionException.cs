namespace AutoBackend.Sdk.Exceptions.Reflection;

internal sealed class InheritanceReflectionException : ReflectionException
{
    internal InheritanceReflectionException()
    {
    }

    internal InheritanceReflectionException(string message) : base(message)
    {
    }

    internal InheritanceReflectionException(string message, Exception innerException) : base(message, innerException)
    {
    }
}