namespace AutoBackend.Sdk.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public sealed class GenericEntityAttribute(params string[] keys) : Attribute
{
    internal string[] Keys { get; } = keys;
}