namespace AutoBackend.Sdk.Exceptions.Reflection;

internal sealed class NotFoundReflectionException : ReflectionException
{
    internal NotFoundReflectionException()
    {
    }

    internal NotFoundReflectionException(string message) : base(message)
    {
    }

    internal NotFoundReflectionException(string message, Exception innerException) : base(message, innerException)
    {
    }
}