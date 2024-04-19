namespace AutoBackend.Sdk.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public sealed class GenericResponseAttribute(params string[] properties) : Attribute
{
    public string[] Properties { get; } = properties;
}