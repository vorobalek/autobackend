namespace AutoBackend.Sdk.Attributes.GenericEntities;

[AttributeUsage(AttributeTargets.Class)]
public class GenericEntityAttribute : Attribute
{
    public GenericEntityAttribute(params string[] keys)
    {
        Keys = keys;
    }

    public string[] Keys { get; }
}