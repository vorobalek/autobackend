using System.Reflection;
using System.Reflection.Emit;
using System.Text.Json.Serialization;
using AutoBackend.Sdk.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AutoBackend.Sdk.Extensions;

internal static class PropertyBuilderExtensions
{
    // ReSharper disable once InconsistentNaming
    internal static void SetGraphQLNameAttribute(this PropertyBuilder propertyBuilder, string name)
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

        propertyBuilder.SetCustomAttribute(graphQlNameAttributeBuilder);
    }

    internal static void SetJsonPropertyAttribute(this PropertyBuilder propertyBuilder, string name)
    {
        var constructorParameters = new[]
        {
            typeof(string)
        };
        var constructorInfo = typeof(JsonPropertyAttribute).GetConstructor(constructorParameters);

        var graphQlNameAttributeBuilder = new CustomAttributeBuilder(
            constructorInfo ?? throw new AutoBackendException(
                $"Unable to determine suitable constructor for type {nameof(JsonPropertyAttribute)}"),
            new object[] { name },
            Array.Empty<PropertyInfo>(),
            Array.Empty<object>());

        propertyBuilder.SetCustomAttribute(graphQlNameAttributeBuilder);
    }

    internal static void SetJsonPropertyNameAttribute(this PropertyBuilder propertyBuilder, string name)
    {
        var constructorParameters = new[]
        {
            typeof(string)
        };
        var constructorInfo = typeof(JsonPropertyNameAttribute).GetConstructor(constructorParameters);

        var graphQlNameAttributeBuilder = new CustomAttributeBuilder(
            constructorInfo ?? throw new AutoBackendException(
                $"Unable to determine suitable constructor for type {nameof(JsonPropertyNameAttribute)}"),
            new object[] { name },
            Array.Empty<PropertyInfo>(),
            Array.Empty<object>());

        propertyBuilder.SetCustomAttribute(graphQlNameAttributeBuilder);
    }

    internal static void SetBindPropertyAttribute(this PropertyBuilder propertyBuilder, string name)
    {
        var constructorParameters = Array.Empty<Type>();
        var constructorInfo = typeof(BindPropertyAttribute).GetConstructor(constructorParameters);

        var nameProperty = typeof(BindPropertyAttribute).GetProperty(nameof(BindPropertyAttribute.Name));

        if (nameProperty is null)
            throw new AutoBackendException(
                $"Unable to determine property {nameof(BindPropertyAttribute.Name)} for attribute {nameof(BindPropertyAttribute)}");

        var graphQlNameAttributeBuilder = new CustomAttributeBuilder(
            constructorInfo ?? throw new AutoBackendException(
                $"Unable to determine suitable constructor for type {nameof(JsonPropertyNameAttribute)}"),
            Array.Empty<object>(),
            new[] { nameProperty },
            new object?[] { name });

        propertyBuilder.SetCustomAttribute(graphQlNameAttributeBuilder);
    }
}