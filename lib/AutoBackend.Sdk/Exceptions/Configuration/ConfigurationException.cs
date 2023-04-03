namespace AutoBackend.Sdk.Exceptions.Configuration;

internal abstract class ConfigurationException : AutoBackendException
{
    protected ConfigurationException()
    {
    }

    protected ConfigurationException(string message) : base(message)
    {
    }

    protected ConfigurationException(string message, Exception innerException) : base(message, innerException)
    {
    }
}