using System.Reflection;
using System.Reflection.Emit;
using AutoBackend.Sdk.Exceptions;

namespace AutoBackend.Sdk.Extensions;

internal static class TypeBuilderExtensions
{
    // ReSharper disable once InconsistentNaming
    internal static void SetGraphQLNameAttribute(this TypeBuilder typeBuilder, string name)
    {
        var constructorParameters = new[]
        {
            typeof(string)
        };
        var constructorInfo = typeof(GraphQLNameAttribute).GetConstructor(constructorParameters);

        var graphQlNameAttributeBuilder = new CustomAttributeBuilder(
            constructorInfo ?? throw new AutoBackendException(
                $"Unable to determine suitable constructor for type {nameof(GraphQLNameAttribute)}"),
            new object[] { name },
            Array.Empty<PropertyInfo>(),
            Array.Empty<object>());

        typeBuilder.SetCustomAttribute(graphQlNameAttributeBuilder);
    }
}