using AutoBackend.Sdk.Controllers.Generic.V1;
using AutoBackend.Sdk.Controllers.Generic.V2;
using Microsoft.AspNetCore.Mvc;

namespace AutoBackend.Sdk.Controllers.Generic;

internal abstract class GenericController : ControllerBase
{
    public static bool IsGenericController(Type type)
    {
        return type.IsGenericType &&
               type.IsAssignableTo(typeof(GenericController));
    }

    private static bool IsGenericControllerV1(Type type)
    {
        return IsGenericController(type) &&
               (
                   type.GetGenericTypeDefinition() == typeof(GenericControllerV1<,>) ||
                   type.GetGenericTypeDefinition() == typeof(GenericControllerV1WithPrimaryKey<,,>) ||
                   type.GetGenericTypeDefinition() == typeof(GenericControllerV1WithComplexKey<,,,>) ||
                   type.GetGenericTypeDefinition() == typeof(GenericControllerV1WithComplexKey<,,,,>) ||
                   type.GetGenericTypeDefinition() == typeof(GenericControllerV1WithComplexKey<,,,,,>) ||
                   type.GetGenericTypeDefinition() == typeof(GenericControllerV1WithComplexKey<,,,,,,>) ||
                   type.GetGenericTypeDefinition() == typeof(GenericControllerV1WithComplexKey<,,,,,,,>) ||
                   type.GetGenericTypeDefinition() == typeof(GenericControllerV1WithComplexKey<,,,,,,,,>) ||
                   type.GetGenericTypeDefinition() == typeof(GenericControllerV1WithComplexKey<,,,,,,,,,>) ||
                   type.GetGenericTypeDefinition() == typeof(GenericControllerV1WithComplexKey<,,,,,,,,,>)
               );
    }

    private static bool IsGenericControllerV2(Type type)
    {
        return IsGenericController(type) &&
               type.GetGenericTypeDefinition() == typeof(GenericControllerV2<,>);
    }

    public static Type GetTargetType(Type controllerType)
    {
        if (IsGenericControllerV1(controllerType)) return controllerType.GenericTypeArguments[0];
        if (IsGenericControllerV2(controllerType)) return controllerType.GenericTypeArguments[0];

        throw new ArgumentOutOfRangeException();
    }
}