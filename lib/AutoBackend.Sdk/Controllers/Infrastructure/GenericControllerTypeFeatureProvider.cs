using System.Reflection;
using AutoBackend.Sdk.Attributes;
using AutoBackend.Sdk.Exceptions;
using AutoBackend.Sdk.Filters.Infrastructure;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace AutoBackend.Sdk.Controllers.Infrastructure;

internal sealed class GenericControllerTypeFeatureProvider : IApplicationFeatureProvider<ControllerFeature>
{
    private readonly Assembly[] _assemblies;

    public GenericControllerTypeFeatureProvider(params Assembly[] assemblies)
    {
        _assemblies = assemblies;
    }

    public void PopulateFeature(IEnumerable<ApplicationPart> parts, ControllerFeature feature)
    {
        foreach (var currentAssembly in _assemblies)
        {
            var candidates = currentAssembly.GetExportedTypes();
            foreach (var candidate in candidates
                         .Where(candidate => candidate
                             .GetCustomAttribute<GenericControllerAttribute>() is not null))
            foreach (var genericControllerType in MakeGenericControllersTypeForCandidate(candidate))
                feature.Controllers.Add(genericControllerType.GetTypeInfo());
        }
    }

    private static IEnumerable<Type> MakeGenericControllersTypeForCandidate(Type candidate)
    {
        if (candidate.GetCustomAttribute<GenericEntityAttribute>() is not { Keys: { } } genericEntityAttribute)
            throw new AutoBackendException(
                $"Generic controller can be generated only for types marked with {nameof(GenericEntityAttribute)} ({candidate.Name}).");

        yield return MakeGenericControllerTypeForCandidate(candidate, genericEntityAttribute);

        if (GenericFilterTypeBuilder.TryBuild(candidate) is { } genericFilterType)
            yield return MakeGenericFilteredControllerTypeForCandidate(candidate, genericFilterType);
    }

    private static Type MakeGenericFilteredControllerTypeForCandidate(Type candidate, Type genericFilterType)
    {
        return typeof(GenericFilteredController<,>)
            .MakeGenericType(
                candidate,
                genericFilterType);
    }

    private static Type MakeGenericControllerTypeForCandidate(Type candidate,
        GenericEntityAttribute genericEntityAttribute)
    {
        var keys = genericEntityAttribute.Keys;
        return keys.Length switch
        {
            0 => typeof(GenericController<>)
                .MakeGenericType(
                    candidate),
            1 => typeof(GenericControllerWithPrimaryKey<,>)
                .MakeGenericType(
                    candidate,
                    GetPropertyTypeOrThrowException(candidate, keys[0])),
            2 => typeof(GenericControllerWithComplexKey<,,>)
                .MakeGenericType(
                    candidate,
                    GetPropertyTypeOrThrowException(candidate, keys[0]),
                    GetPropertyTypeOrThrowException(candidate, keys[1])),
            3 => typeof(GenericControllerWithComplexKey<,,,>)
                .MakeGenericType(
                    candidate,
                    GetPropertyTypeOrThrowException(candidate, keys[0]),
                    GetPropertyTypeOrThrowException(candidate, keys[1]),
                    GetPropertyTypeOrThrowException(candidate, keys[2])),
            4 => typeof(GenericControllerWithComplexKey<,,,,>)
                .MakeGenericType(
                    candidate,
                    GetPropertyTypeOrThrowException(candidate, keys[0]),
                    GetPropertyTypeOrThrowException(candidate, keys[1]),
                    GetPropertyTypeOrThrowException(candidate, keys[2]),
                    GetPropertyTypeOrThrowException(candidate, keys[3])),
            5 => typeof(GenericControllerWithComplexKey<,,,,,>)
                .MakeGenericType(
                    candidate,
                    GetPropertyTypeOrThrowException(candidate, keys[0]),
                    GetPropertyTypeOrThrowException(candidate, keys[1]),
                    GetPropertyTypeOrThrowException(candidate, keys[2]),
                    GetPropertyTypeOrThrowException(candidate, keys[3]),
                    GetPropertyTypeOrThrowException(candidate, keys[4])),
            6 => typeof(GenericControllerWithComplexKey<,,,,,,>)
                .MakeGenericType(
                    candidate,
                    GetPropertyTypeOrThrowException(candidate, keys[0]),
                    GetPropertyTypeOrThrowException(candidate, keys[1]),
                    GetPropertyTypeOrThrowException(candidate, keys[2]),
                    GetPropertyTypeOrThrowException(candidate, keys[3]),
                    GetPropertyTypeOrThrowException(candidate, keys[4]),
                    GetPropertyTypeOrThrowException(candidate, keys[5])),
            7 => typeof(GenericControllerWithComplexKey<,,,,,,,>)
                .MakeGenericType(
                    candidate,
                    GetPropertyTypeOrThrowException(candidate, keys[0]),
                    GetPropertyTypeOrThrowException(candidate, keys[1]),
                    GetPropertyTypeOrThrowException(candidate, keys[2]),
                    GetPropertyTypeOrThrowException(candidate, keys[3]),
                    GetPropertyTypeOrThrowException(candidate, keys[4]),
                    GetPropertyTypeOrThrowException(candidate, keys[5]),
                    GetPropertyTypeOrThrowException(candidate, keys[6])),
            8 => typeof(GenericControllerWithComplexKey<,,,,,,,,>)
                .MakeGenericType(
                    candidate,
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