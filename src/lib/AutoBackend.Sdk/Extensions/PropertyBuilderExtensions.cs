using System.Reflection.Emit;
using System.Text.Json.Serialization;
using AutoBackend.Sdk.Builders;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AutoBackend.Sdk.Extensions;

internal static class PropertyBuilderExtensions
{
    // ReSharper disable once InconsistentNaming
    extension(PropertyBuilder propertyBuilder)
    {
        internal void SetGraphQLNameAttribute(string name)
        {
            propertyBuilder.SetAttribute<GraphQLNameAttribute>(args: name);
        }

        internal void SetJsonPropertyAttribute(string name)
        {
            propertyBuilder.SetAttribute<JsonPropertyAttribute>(args: name);
        }

        internal void SetJsonPropertyNameAttribute(string name)
        {
            propertyBuilder.SetAttribute<JsonPropertyNameAttribute>(args: name);
        }

        internal void SetBindPropertyAttribute(string name)
        {
            propertyBuilder.SetAttribute<BindPropertyAttribute>(
                new Dictionary<string, object?>
                {
                    [nameof(BindPropertyAttribute.Name)] = name
                });
        }

        private void SetAttribute<TAttribute>(IReadOnlyDictionary<string, object?>? properties = null,
            IReadOnlyDictionary<string, object?>? fields = null,
            params object[] args)
            where TAttribute : Attribute
        {
            propertyBuilder.SetCustomAttribute(AttributeBuilder.Create<TAttribute>(properties, fields, args));
        }
    }

    // ReSharper disable once MemberCanBePrivate.Global
}