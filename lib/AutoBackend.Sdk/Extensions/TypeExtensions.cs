using AutoBackend.Sdk.Controllers;

namespace AutoBackend.Sdk.Extensions;

internal static class TypeExtensions
{
    internal static bool IsGenericControllerV1(this Type type)
    {
        return type.IsGenericType &&
               (
                   type.GetGenericTypeDefinition() == typeof(GenericController<>) ||
                   type.GetGenericTypeDefinition() == typeof(GenericFilteredController<,>) ||
                   type.GetGenericTypeDefinition() == typeof(GenericControllerWithPrimaryKey<,>) ||
                   type.GetGenericTypeDefinition() == typeof(GenericControllerWithComplexKey<,,>) ||
                   type.GetGenericTypeDefinition() == typeof(GenericControllerWithComplexKey<,,,>) ||
                   type.GetGenericTypeDefinition() == typeof(GenericControllerWithComplexKey<,,,,>) ||
                   type.GetGenericTypeDefinition() == typeof(GenericControllerWithComplexKey<,,,,,>) ||
                   type.GetGenericTypeDefinition() == typeof(GenericControllerWithComplexKey<,,,,,,>) ||
                   type.GetGenericTypeDefinition() == typeof(GenericControllerWithComplexKey<,,,,,,,>) ||
                   type.GetGenericTypeDefinition() == typeof(GenericControllerWithComplexKey<,,,,,,,,>) ||
                   type.GetGenericTypeDefinition() == typeof(GenericControllerWithComplexKey<,,,,,,,,>)
               );
    }
}