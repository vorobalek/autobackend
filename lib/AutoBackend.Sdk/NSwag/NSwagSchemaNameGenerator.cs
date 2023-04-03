using NJsonSchema.Generation;

namespace AutoBackend.Sdk.NSwag;

internal sealed class NSwagSchemaNameGenerator : ISchemaNameGenerator
{
    public string Generate(Type type)
    {
        var typeName = type.Name;
        if (!type.IsGenericType) return typeName;
        var genericArgs = string.Join(", ", type.GetGenericArguments().Select(Generate));

        var index = typeName.IndexOf('`');
        var typeNameWithoutGenericArity = index == -1 ? typeName : typeName[..index];

        return string.Format(
            Constants.GenericTypeBeautifulName,
            typeNameWithoutGenericArity,
            genericArgs);
    }
}