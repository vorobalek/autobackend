using System.Reflection;
using System.Reflection.Emit;
using System.Text.Json.Serialization;
using AutoBackend.Sdk.Exceptions.Reflection;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using GraphQLIgnoreAttribute = HotChocolate.GraphQLIgnoreAttribute;
using JsonIgnoreAttribute = System.Text.Json.Serialization.JsonIgnoreAttribute;

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

        var attributeBuilder = new CustomAttributeBuilder(
            constructorInfo ?? throw new NotFoundReflectionException(
                string.Format(
                    Constants.UnableToFindASuitableConstructorForTheType,
                    nameof(GraphQLNameAttribute))),
            [name],
            Array.Empty<PropertyInfo>(),
            Array.Empty<object>());

        propertyBuilder.SetCustomAttribute(attributeBuilder);
    }

    internal static void SetGraphQLIgnoreAttribute(this PropertyBuilder propertyBuilder)
    {
        var constructorParameters = Array.Empty<Type>();
        var constructorInfo = typeof(GraphQLIgnoreAttribute).GetConstructor(constructorParameters);

        var attributeBuilder = new CustomAttributeBuilder(
            constructorInfo ?? throw new NotFoundReflectionException(
                string.Format(
                    Constants.UnableToFindASuitableConstructorForTheType,
                    nameof(GraphQLIgnoreAttribute))),
            Array.Empty<object>(),
            Array.Empty<PropertyInfo>(),
            Array.Empty<object>());

        propertyBuilder.SetCustomAttribute(attributeBuilder);
    }

    internal static void SetJsonPropertyAttribute(this PropertyBuilder propertyBuilder, string name)
    {
        var constructorParameters = new[]
        {
            typeof(string)
        };
        var constructorInfo = typeof(JsonPropertyAttribute).GetConstructor(constructorParameters);

        var attributeBuilder = new CustomAttributeBuilder(
            constructorInfo ?? throw new NotFoundReflectionException(
                string.Format(
                    Constants.UnableToFindASuitableConstructorForTheType,
                    nameof(JsonPropertyAttribute))),
            [name],
            Array.Empty<PropertyInfo>(),
            Array.Empty<object>());

        propertyBuilder.SetCustomAttribute(attributeBuilder);
    }

    internal static void SetSystemTextJsonIgnoreAttribute(this PropertyBuilder propertyBuilder)
    {
        var constructorParameters = Array.Empty<Type>();
        var constructorInfo = typeof(JsonIgnoreAttribute).GetConstructor(constructorParameters);

        var attributeBuilder = new CustomAttributeBuilder(
            constructorInfo ?? throw new NotFoundReflectionException(
                string.Format(
                    Constants.UnableToFindASuitableConstructorForTheType,
                    nameof(JsonIgnoreAttribute))),
            Array.Empty<object>(),
            Array.Empty<PropertyInfo>(),
            Array.Empty<object>());

        propertyBuilder.SetCustomAttribute(attributeBuilder);
    }

    internal static void SetNewtonsoftJsonIgnoreAttribute(this PropertyBuilder propertyBuilder)
    {
        var constructorParameters = Array.Empty<Type>();
        var constructorInfo = typeof(Newtonsoft.Json.JsonIgnoreAttribute).GetConstructor(constructorParameters);

        var attributeBuilder = new CustomAttributeBuilder(
            constructorInfo ?? throw new NotFoundReflectionException(
                string.Format(
                    Constants.UnableToFindASuitableConstructorForTheType,
                    nameof(Newtonsoft.Json.JsonIgnoreAttribute))),
            Array.Empty<object>(),
            Array.Empty<PropertyInfo>(),
            Array.Empty<object>());

        propertyBuilder.SetCustomAttribute(attributeBuilder);
    }

    internal static void SetJsonPropertyNameAttribute(this PropertyBuilder propertyBuilder, string name)
    {
        var constructorParameters = new[]
        {
            typeof(string)
        };
        var constructorInfo = typeof(JsonPropertyNameAttribute).GetConstructor(constructorParameters);

        var attributeBuilder = new CustomAttributeBuilder(
            constructorInfo ?? throw new NotFoundReflectionException(
                string.Format(
                    Constants.UnableToFindASuitableConstructorForTheType,
                    nameof(JsonPropertyNameAttribute))),
            [name],
            Array.Empty<PropertyInfo>(),
            Array.Empty<object>());

        propertyBuilder.SetCustomAttribute(attributeBuilder);
    }

    internal static void SetBindPropertyAttribute(this PropertyBuilder propertyBuilder, string name)
    {
        var constructorParameters = Array.Empty<Type>();
        var constructorInfo = typeof(BindPropertyAttribute).GetConstructor(constructorParameters);

        var nameProperty = typeof(BindPropertyAttribute).GetProperty(nameof(BindPropertyAttribute.Name));

        if (nameProperty is null)
            throw new NotFoundReflectionException(
                string.Format(
                    Constants.UnableToFindAPropertyWithNameInObject,
                    nameof(BindPropertyAttribute.Name),
                    nameof(BindPropertyAttribute)));

        var attributeBuilder = new CustomAttributeBuilder(
            constructorInfo ?? throw new NotFoundReflectionException(
                string.Format(
                    Constants.UnableToFindASuitableConstructorForTheType,
                    nameof(JsonPropertyNameAttribute))),
            Array.Empty<object>(),
            [nameProperty],
            [name]);

        propertyBuilder.SetCustomAttribute(attributeBuilder);
    }
}