namespace AutoBackend.Sdk.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public sealed class GenericRequestAttribute(params string[] properties) : Attribute
{
    public string[] Properties { get; } = properties;
}