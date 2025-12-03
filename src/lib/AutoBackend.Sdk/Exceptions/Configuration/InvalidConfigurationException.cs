namespace AutoBackend.Sdk.Exceptions.Configuration;

internal class InvalidConfigurationException : ConfigurationException
{
    internal InvalidConfigurationException(string message) : base(message)
    {
    }
}