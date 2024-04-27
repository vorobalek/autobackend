using System.Reflection;
using AutoBackend.Sdk.Controllers;

namespace AutoBackend.Sdk.Extensions;

internal static class TypeExtensions
{
    private static readonly Type[] BuiltInValueTypes =
    [
        typeof(bool),
        typeof(byte),
        typeof(sbyte),
        typeof(char),
        typeof(decimal),
        typeof(double),
        typeof(float),
        typeof(int),
        typeof(uint),
        typeof(nint),
        typeof(nuint),
        typeof(long),
        typeof(ulong),
        typeof(short),
        typeof(ushort)
    ];

    private static readonly Type[] BuiltInStructures =
    [
        typeof(Guid),
        typeof(DateOnly),
        typeof(DateTime),
        typeof(TimeSpan),
        typeof(DateTimeOffset)
    ];

    internal static bool IsGenericControllerV1(this Type type)
    {
        return type.IsGenericType &&
               (
                   type.GetGenericTypeDefinition() == typeof(GenericController<,,>) ||
                   type.GetGenericTypeDefinition() == typeof(GenericControllerWithNoKey<,,,>) ||
                   type.GetGenericTypeDefinition() == typeof(GenericControllerWithPrimaryKey<,,,,>) ||
                   type.GetGenericTypeDefinition() == typeof(GenericControllerWithComplexKey<,,,,,>) ||
                   type.GetGenericTypeDefinition() == typeof(GenericControllerWithComplexKey<,,,,,,>) ||
                   type.GetGenericTypeDefinition() == typeof(GenericControllerWithComplexKey<,,,,,,,>) ||
                   type.GetGenericTypeDefinition() == typeof(GenericControllerWithComplexKey<,,,,,,,,>) ||
                   type.GetGenericTypeDefinition() == typeof(GenericControllerWithComplexKey<,,,,,,,,,>) ||
                   type.GetGenericTypeDefinition() == typeof(GenericControllerWithComplexKey<,,,,,,,,,,>) ||
                   type.GetGenericTypeDefinition() == typeof(GenericControllerWithComplexKey<,,,,,,,,,,,>)
               );
    }

    internal static bool IsPrimitiveModelType(this Type type)
    {
        if (BuiltInValueTypes.Contains(type))
            return true;

        if (Nullable.GetUnderlyingType(type) is { } underlyingType &&
            (BuiltInValueTypes.Contains(underlyingType) ||
             BuiltInStructures.Contains(underlyingType))) return true;

        if (BuiltInStructures.Contains(type))
            return true;

        if (type == typeof(string))
            return true;

        if (type.IsEnum)
            return true;

        return false;
    }

    internal static bool IsCollection(this Type type)
    {
        return (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(ICollection<>)) ||
               type.GetTypeInfo().ImplementedInterfaces.Any(i =>
                   i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICollection<>));
    }

    internal static Type? GetCollectionType(this Type type)
    {
        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(ICollection<>))
            return type.GetGenericArguments()[0];

        if (type.GetTypeInfo().ImplementedInterfaces.SingleOrDefault(i =>
                i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICollection<>)) is { } iEnumerableGeneric)
            return iEnumerableGeneric.GetGenericArguments()[0];

        return null;
    }
}