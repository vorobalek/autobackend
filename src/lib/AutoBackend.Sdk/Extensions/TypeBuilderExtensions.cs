using System.Reflection.Emit;
using AutoBackend.Sdk.Builders;

namespace AutoBackend.Sdk.Extensions;

internal static class TypeBuilderExtensions
{
    // ReSharper disable once InconsistentNaming
    internal static void SetGraphQLNameAttribute(this TypeBuilder typeBuilder, string name)
    {
        typeBuilder.SetAttribute<GraphQLNameAttribute>(args: name);
    }

    // ReSharper disable once MemberCanBePrivate.Global
    internal static void SetAttribute<TAttribute>(
        this TypeBuilder typeBuilder,
        IReadOnlyDictionary<string, object?>? properties = null,
        IReadOnlyDictionary<string, object?>? fields = null,
        params object[] args)
        where TAttribute : Attribute
    {
        typeBuilder.SetCustomAttribute(AttributeBuilder.Create<TAttribute>(properties, fields, args));
    }
}