using NJsonSchema;

namespace AutoBackend.Sdk.NSwag;

internal sealed class NSwagTypeNameGenerator : DefaultTypeNameGenerator
{
    public override string Generate(JsonSchema schema, string typeNameHint, IEnumerable<string> reservedTypeNames)
    {
        if (string.IsNullOrWhiteSpace(typeNameHint) && !string.IsNullOrWhiteSpace(schema.DocumentPath))
            typeNameHint = schema.DocumentPath.Replace("\\", "/").Split('/').Last();

        return typeNameHint;
    }
}