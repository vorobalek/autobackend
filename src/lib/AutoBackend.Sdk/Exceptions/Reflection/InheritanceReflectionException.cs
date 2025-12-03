namespace AutoBackend.Sdk.Exceptions.Reflection;

internal sealed class InheritanceReflectionException : ReflectionException
{
    internal InheritanceReflectionException()
    {
    }

    internal InheritanceReflectionException(string message) : base(message)
    {
    }
}