namespace AutoBackend.Sdk.Attributes.GenericControllers;

[AttributeUsage(AttributeTargets.Class)]
public abstract class GenericControllerAttribute : Attribute
{
    public abstract string Version { get; }
    public abstract Type[] TargetGenericControllerTypes { get; }
    public abstract Type MakeControllerTypeForCandidate(Type candidate);
}