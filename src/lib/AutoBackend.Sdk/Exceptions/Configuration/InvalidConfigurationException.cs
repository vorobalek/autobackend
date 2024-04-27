namespace AutoBackend.Sdk.Exceptions.Configuration;

internal class InvalidConfigurationException : ConfigurationException
{
    internal InvalidConfigurationException()
    {
    }

    internal InvalidConfigurationException(string message) : base(message)
    {
    }

    internal InvalidConfigurationException(string message, Exception innerException) : base(message, innerException)
    {
    }
}