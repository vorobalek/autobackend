using System.Reflection.Emit;
using AutoBackend.Sdk.Builders;

namespace AutoBackend.Sdk.Extensions;

internal static class TypeBuilderExtensions
{
    // ReSharper disable once InconsistentNaming
    extension(TypeBuilder typeBuilder)
    {
        internal void SetGraphQLNameAttribute(string name)
        {
            typeBuilder.SetAttribute<GraphQLNameAttribute>(args: name);
        }

        private void SetAttribute<TAttribute>(IReadOnlyDictionary<string, object?>? properties = null,
            IReadOnlyDictionary<string, object?>? fields = null,
            params object[] args)
            where TAttribute : Attribute
        {
            typeBuilder.SetCustomAttribute(AttributeBuilder.Create<TAttribute>(properties, fields, args));
        }
    }

    // ReSharper disable once MemberCanBePrivate.Global
}