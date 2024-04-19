using System.Reflection;
using System.Reflection.Emit;
using AutoBackend.Sdk.Exceptions.Reflection;

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
            constructorInfo ?? throw new NotFoundReflectionException(
                string.Format(
                    Constants.UnableToFindASuitableConstructorForTheType,
                    nameof(GraphQLNameAttribute))),
            [name],
            Array.Empty<PropertyInfo>(),
            Array.Empty<object>());

        typeBuilder.SetCustomAttribute(graphQlNameAttributeBuilder);
    }
}