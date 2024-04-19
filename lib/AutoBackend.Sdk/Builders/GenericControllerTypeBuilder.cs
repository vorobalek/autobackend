using System.Reflection;
using AutoBackend.Sdk.Attributes;
using AutoBackend.Sdk.Controllers;
using AutoBackend.Sdk.Exceptions.Reflection;

namespace AutoBackend.Sdk.Builders;

internal static class GenericControllerTypeBuilder
{
    internal static Type BuildForCandidate(Type candidate)
    {
        if (candidate.GetCustomAttribute<GenericEntityAttribute>() is not { Keys: not null } genericEntityAttribute)
            throw new NotFoundReflectionException(
                string.Format(
                    Constants.AGenericControllerCanBeGeneratedOnlyForTypesMarkedWith,
                    nameof(GenericEntityAttribute),
                    candidate.Name));

        var genericFilterType = GenericFilterTypeBuilder.BuildForCandidate(candidate);
        var genericRequestType = GenericRequestTypeBuilder.BuildForCandidate(candidate);
        var genericResponseType = GenericResponseTypeBuilder.BuildForCandidate(candidate);
        var keys = genericEntityAttribute.Keys;
        return keys.Length switch
        {
            0 => typeof(GenericControllerWithNoKey<,,,>)
                .MakeGenericType(
                    candidate,
                    genericRequestType,
                    genericResponseType,
                    genericFilterType),
            1 => typeof(GenericControllerWithPrimaryKey<,,,,>)
                .MakeGenericType(
                    candidate,
                    genericRequestType,
                    genericResponseType,
                    genericFilterType,
                    GetPropertyTypeOrThrowException(candidate, keys[0])),
            2 => typeof(GenericControllerWithComplexKey<,,,,,>)
                .MakeGenericType(
                    candidate,
                    genericRequestType,
                    genericResponseType,
                    genericFilterType,
                    GetPropertyTypeOrThrowException(candidate, keys[0]),
                    GetPropertyTypeOrThrowException(candidate, keys[1])),
            3 => typeof(GenericControllerWithComplexKey<,,,,,,>)
                .MakeGenericType(
                    candidate,
                    genericRequestType,
                    genericResponseType,
                    genericFilterType,
                    GetPropertyTypeOrThrowException(candidate, keys[0]),
                    GetPropertyTypeOrThrowException(candidate, keys[1]),
                    GetPropertyTypeOrThrowException(candidate, keys[2])),
            4 => typeof(GenericControllerWithComplexKey<,,,,,,,>)
                .MakeGenericType(
                    candidate,
                    genericRequestType,
                    genericResponseType,
                    genericFilterType,
                    GetPropertyTypeOrThrowException(candidate, keys[0]),
                    GetPropertyTypeOrThrowException(candidate, keys[1]),
                    GetPropertyTypeOrThrowException(candidate, keys[2]),
                    GetPropertyTypeOrThrowException(candidate, keys[3])),
            5 => typeof(GenericControllerWithComplexKey<,,,,,,,,>)
                .MakeGenericType(
                    candidate,
                    genericRequestType,
                    genericResponseType,
                    genericFilterType,
                    GetPropertyTypeOrThrowException(candidate, keys[0]),
                    GetPropertyTypeOrThrowException(candidate, keys[1]),
                    GetPropertyTypeOrThrowException(candidate, keys[2]),
                    GetPropertyTypeOrThrowException(candidate, keys[3]),
                    GetPropertyTypeOrThrowException(candidate, keys[4])),
            6 => typeof(GenericControllerWithComplexKey<,,,,,,,,,>)
                .MakeGenericType(
                    candidate,
                    genericRequestType,
                    genericResponseType,
                    genericFilterType,
                    GetPropertyTypeOrThrowException(candidate, keys[0]),
                    GetPropertyTypeOrThrowException(candidate, keys[1]),
                    GetPropertyTypeOrThrowException(candidate, keys[2]),
                    GetPropertyTypeOrThrowException(candidate, keys[3]),
                    GetPropertyTypeOrThrowException(candidate, keys[4]),
                    GetPropertyTypeOrThrowException(candidate, keys[5])),
            7 => typeof(GenericControllerWithComplexKey<,,,,,,,,,,>)
                .MakeGenericType(
                    candidate,
                    genericRequestType,
                    genericResponseType,
                    genericFilterType,
                    GetPropertyTypeOrThrowException(candidate, keys[0]),
                    GetPropertyTypeOrThrowException(candidate, keys[1]),
                    GetPropertyTypeOrThrowException(candidate, keys[2]),
                    GetPropertyTypeOrThrowException(candidate, keys[3]),
                    GetPropertyTypeOrThrowException(candidate, keys[4]),
                    GetPropertyTypeOrThrowException(candidate, keys[5]),
                    GetPropertyTypeOrThrowException(candidate, keys[6])),
            8 => typeof(GenericControllerWithComplexKey<,,,,,,,,,,,>)
                .MakeGenericType(
                    candidate,
                    genericRequestType,
                    genericResponseType,
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
               ?? throw new NotFoundReflectionException(
                   string.Format(
                       Constants.UnableToBuildAGenericControllerForTypeThePropertyHasNotBeenFound,
                       candidate.FullName, key));
    }
}