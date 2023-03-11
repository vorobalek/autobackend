namespace AutoBackend.Sdk.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public sealed class GenericEntityAttribute : Attribute
{
    public GenericEntityAttribute(params string[] keys)
    {
        Keys = keys;
    }

    internal string[] Keys { get; }
}