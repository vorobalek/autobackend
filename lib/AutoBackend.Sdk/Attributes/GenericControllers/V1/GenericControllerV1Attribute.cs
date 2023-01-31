using System.Reflection;
using AutoBackend.Sdk.Attributes.GenericEntities;
using AutoBackend.Sdk.Controllers.Generic.V1;
using AutoBackend.Sdk.Exceptions;
using AutoBackend.Sdk.Filters.Generic.Infrastructure;

namespace AutoBackend.Sdk.Attributes.GenericControllers.V1;

[AttributeUsage(AttributeTargets.Class)]
public sealed class GenericControllerV1Attribute : GenericControllerAttribute
{
    public override string Version => "v1";

    public override Type[] TargetGenericControllerTypes => new[]
    {
        typeof(GenericControllerV1<,>),
        typeof(GenericControllerV1WithPrimaryKey<,,>),
        typeof(GenericControllerV1WithComplexKey<,,,>),
        typeof(GenericControllerV1WithComplexKey<,,,,>),
        typeof(GenericControllerV1WithComplexKey<,,,,,>),
        typeof(GenericControllerV1WithComplexKey<,,,,,,>),
        typeof(GenericControllerV1WithComplexKey<,,,,,,,>),
        typeof(GenericControllerV1WithComplexKey<,,,,,,,,>),
        typeof(GenericControllerV1WithComplexKey<,,,,,,,,,>)
    };

    public override Type MakeControllerTypeForCandidate(Type candidate)
    {
        if (candidate.GetCustomAttribute<GenericEntityAttribute>() is not { Keys: { } } genericEntityAttribute)
            throw new AutoBackendException(
                $"Generic controller can be generated only for types marked with {nameof(GenericEntityAttribute)}");

        var genericFilterType = GenericFilterTypeBuilder.Build(candidate);
        var keys = genericEntityAttribute.Keys;
        return keys.Length switch
        {
            0 => typeof(GenericControllerV1<,>)
                .MakeGenericType(
                    candidate,
                    genericFilterType),
            1 => typeof(GenericControllerV1WithPrimaryKey<,,>)
                .MakeGenericType(
                    candidate,
                    genericFilterType,
                    GetPropertyTypeOrThrowException(candidate, keys[0])),
            2 => typeof(GenericControllerV1WithComplexKey<,,,>)
                .MakeGenericType(
                    candidate,
                    genericFilterType,
                    GetPropertyTypeOrThrowException(candidate, keys[0]),
                    GetPropertyTypeOrThrowException(candidate, keys[1])),
            3 => typeof(GenericControllerV1WithComplexKey<,,,,>)
                .MakeGenericType(
                    candidate,
                    genericFilterType,
                    GetPropertyTypeOrThrowException(candidate, keys[0]),
                    GetPropertyTypeOrThrowException(candidate, keys[1]),
                    GetPropertyTypeOrThrowException(candidate, keys[2])),
            4 => typeof(GenericControllerV1WithComplexKey<,,,,,>)
                .MakeGenericType(
                    candidate,
                    genericFilterType,
                    GetPropertyTypeOrThrowException(candidate, keys[0]),
                    GetPropertyTypeOrThrowException(candidate, keys[1]),
                    GetPropertyTypeOrThrowException(candidate, keys[2]),
                    GetPropertyTypeOrThrowException(candidate, keys[3])),
            5 => typeof(GenericControllerV1WithComplexKey<,,,,,,>)
                .MakeGenericType(
                    candidate,
                    genericFilterType,
                    GetPropertyTypeOrThrowException(candidate, keys[0]),
                    GetPropertyTypeOrThrowException(candidate, keys[1]),
                    GetPropertyTypeOrThrowException(candidate, keys[2]),
                    GetPropertyTypeOrThrowException(candidate, keys[3]),
                    GetPropertyTypeOrThrowException(candidate, keys[4])),
            6 => typeof(GenericControllerV1WithComplexKey<,,,,,,,>)
                .MakeGenericType(
                    candidate,
                    genericFilterType,
                    GetPropertyTypeOrThrowException(candidate, keys[0]),
                    GetPropertyTypeOrThrowException(candidate, keys[1]),
                    GetPropertyTypeOrThrowException(candidate, keys[2]),
                    GetPropertyTypeOrThrowException(candidate, keys[3]),
                    GetPropertyTypeOrThrowException(candidate, keys[4]),
                    GetPropertyTypeOrThrowException(candidate, keys[5])),
            7 => typeof(GenericControllerV1WithComplexKey<,,,,,,,,>)
                .MakeGenericType(
                    candidate,
                    genericFilterType,
                    GetPropertyTypeOrThrowException(candidate, keys[0]),
                    GetPropertyTypeOrThrowException(candidate, keys[1]),
                    GetPropertyTypeOrThrowException(candidate, keys[2]),
                    GetPropertyTypeOrThrowException(candidate, keys[3]),
                    GetPropertyTypeOrThrowException(candidate, keys[4]),
                    GetPropertyTypeOrThrowException(candidate, keys[5]),
                    GetPropertyTypeOrThrowException(candidate, keys[6])),
            8 => typeof(GenericControllerV1WithComplexKey<,,,,,,,,,>)
                .MakeGenericType(
                    candidate,
                    genericFilterType,
                    GetPropertyTypeOrThrowException(candidate, keys[0]),
                    GetPropertyTypeOrThrowException(candidate, keys[1]),
                    GetPropertyTypeOrThrowException(candidate, keys[2]),
                    GetPropertyTypeOrThrowException(candidate, keys[3]),
                    GetPropertyTypeOrThrowException(candidate, keys[4]),
                    GetPropertyTypeOrThrowException(candidate, keys[5]),
                    GetPropertyTypeOrThrowException(candidate, keys[6]),
                    GetPropertyTypeOrThrowException(candidate, keys[7])),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private static Type GetPropertyTypeOrThrowException(Type candidate, string key)
    {
        return candidate
                   .GetProperty(key)
                   ?.PropertyType
               ?? throw new AutoBackendException(
                   $"Unable to build generic controller for type {candidate.FullName}. Unable to determine type of property {key}. Property not found.");
    }
}