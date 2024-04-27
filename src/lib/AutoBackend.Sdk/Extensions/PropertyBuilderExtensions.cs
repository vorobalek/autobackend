using System.Reflection.Emit;
using System.Text.Json.Serialization;
using AutoBackend.Sdk.Builders;
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
        propertyBuilder.SetAttribute<GraphQLNameAttribute>(args: name);
    }

    internal static void SetGraphQLIgnoreAttribute(this PropertyBuilder propertyBuilder)
    {
        propertyBuilder.SetAttribute<GraphQLIgnoreAttribute>();
    }

    internal static void SetJsonPropertyAttribute(this PropertyBuilder propertyBuilder, string name)
    {
        propertyBuilder.SetAttribute<JsonPropertyAttribute>(args: name);
    }

    internal static void SetSystemTextJsonIgnoreAttribute(this PropertyBuilder propertyBuilder)
    {
        propertyBuilder.SetAttribute<JsonIgnoreAttribute>();
    }

    internal static void SetNewtonsoftJsonIgnoreAttribute(this PropertyBuilder propertyBuilder)
    {
        propertyBuilder.SetAttribute<Newtonsoft.Json.JsonIgnoreAttribute>();
    }

    internal static void SetJsonPropertyNameAttribute(this PropertyBuilder propertyBuilder, string name)
    {
        propertyBuilder.SetAttribute<JsonPropertyNameAttribute>(args: name);
    }

    internal static void SetBindPropertyAttribute(this PropertyBuilder propertyBuilder, string name)
    {
        propertyBuilder.SetAttribute<BindPropertyAttribute>(
            new Dictionary<string, object?>
            {
                [nameof(BindPropertyAttribute.Name)] = name
            });
    }

    // ReSharper disable once MemberCanBePrivate.Global
    internal static void SetAttribute<TAttribute>(
        this PropertyBuilder propertyBuilder,
        IReadOnlyDictionary<string, object?>? properties = null,
        IReadOnlyDictionary<string, object?>? fields = null,
        params object[] args)
        where TAttribute : Attribute
    {
        propertyBuilder.SetCustomAttribute(AttributeBuilder.Create<TAttribute>(properties, fields, args));
    }
}