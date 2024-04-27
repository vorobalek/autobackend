using System.Reflection;
using System.Reflection.Emit;
using AutoBackend.Sdk.Attributes;
using AutoBackend.Sdk.Exceptions.Reflection;
using AutoBackend.Sdk.Extensions;
using AutoBackend.Sdk.GraphQL.Mutations;

namespace AutoBackend.Sdk.Builders;

internal static class GenericGqlMutationTypeBuilder
{
    private static readonly ModuleBuilder ModuleBuilder =
        AssemblyBuilder
            .DefineDynamicAssembly(
                new AssemblyName(Constants.GenericGqlMutationsAssemblyName),
                AssemblyBuilderAccess.Run)
            .DefineDynamicModule(Constants.GenericGqlMutationsModuleName);

    internal static Type Build(params Assembly[] assemblies)
    {
        var mutationTypeName = Constants.GenericGqlMutationTypeName;
        var mutationTypeBuilder = ModuleBuilder.DefineType(
            mutationTypeName,
            TypeAttributes.Public |
            TypeAttributes.Class |
            TypeAttributes.AutoClass |
            TypeAttributes.AnsiClass |
            TypeAttributes.BeforeFieldInit |
            TypeAttributes.AutoLayout,
            null);

        mutationTypeBuilder.DefineDefaultConstructor(
            MethodAttributes.Public |
            MethodAttributes.HideBySig |
            MethodAttributes.SpecialName);

        foreach (var currentAssembly in assemblies)
        {
            var candidates = currentAssembly.GetExportedTypes();
            foreach (var candidate in candidates
                         .Where(candidate => candidate
                             .GetCustomAttribute<GenericGqlMutationAttribute>() is not null))
                BuildProperty(
                    mutationTypeBuilder,
                    candidate);
        }

        mutationTypeBuilder.SetGraphQLNameAttribute(mutationTypeName);
        var mutationType = mutationTypeBuilder.CreateType();

        return mutationType;
    }

    private static void BuildProperty(
        TypeBuilder mutationTypeBuilder,
        Type propertyCandidateType)
    {
        var propertyTypeParent = GetPropertyTypeForCandidate(propertyCandidateType);

        var propertyTypeName = string.Format(
            Constants.GenericGqlMutationPropertyTypeName,
            propertyCandidateType.Name);
        var propertyTypeBuilder = ModuleBuilder
            .DefineType(
                propertyTypeName,
                TypeAttributes.Public |
                TypeAttributes.Class |
                TypeAttributes.AutoClass |
                TypeAttributes.AnsiClass |
                TypeAttributes.BeforeFieldInit |
                TypeAttributes.AutoLayout,
                propertyTypeParent);

        propertyTypeBuilder.DefineDefaultConstructor(
            MethodAttributes.Public |
            MethodAttributes.HideBySig |
            MethodAttributes.SpecialName);

        propertyTypeBuilder.SetGraphQLNameAttribute(propertyTypeName);

        var propertyType = propertyTypeBuilder.CreateType();

        var propertyName = propertyCandidateType.Name;
        var propertyBuilder = mutationTypeBuilder
            .DefineProperty(
                propertyName,
                PropertyAttributes.None,
                propertyType,
                null);

        var getMethod = mutationTypeBuilder
            .DefineMethod(
                string.Format(
                    Constants.PropertyGetterName,
                    propertyName),
                MethodAttributes.Public |
                MethodAttributes.SpecialName |
                MethodAttributes.HideBySig,
                propertyType,
                Type.EmptyTypes);

        var constructorInfo = propertyType.GetConstructor(Array.Empty<Type>());

        var getMethodIl = getMethod.GetILGenerator();
        getMethodIl.Emit(OpCodes.Newobj, constructorInfo ?? throw new NotFoundReflectionException(
            string.Format(Constants.UnableToFindASuitableConstructorForTheType, propertyType.Name)));
        getMethodIl.Emit(OpCodes.Ret);

        propertyBuilder.SetGetMethod(getMethod);

        propertyBuilder.SetGraphQLNameAttribute(propertyName.ToCamelCase());
    }

    private static Type GetPropertyTypeForCandidate(Type candidate)
    {
        if (candidate.GetCustomAttribute<GenericEntityAttribute>() is not { Keys: not null } genericEntityAttribute)
            throw new NotFoundReflectionException(
                string.Format(
                    Constants.AGenericGraphQlMutationCanBeGeneratedOnlyForTypesMarkedWith,
                    nameof(GenericEntityAttribute), candidate.Name));

        var genericFilterType = GenericFilterTypeBuilder.BuildForCandidate(candidate);
        var genericRequestType = GenericRequestTypeBuilder.BuildForCandidate(candidate);
        var genericResponseType = GenericResponseTypeBuilder.BuildForCandidate(candidate);
        var keys = genericEntityAttribute.Keys;
        return keys.Length switch
        {
            0 => typeof(GenericGqlMutationWithNoKey<,,,>)
                .MakeGenericType(
                    candidate,
                    genericRequestType,
                    genericResponseType,
                    genericFilterType),
            1 => typeof(GenericGqlMutationWithPrimaryKey<,,,,>)
                .MakeGenericType(
                    candidate,
                    genericRequestType,
                    genericResponseType,
                    genericFilterType,
                    GetPropertyTypeOrThrowException(candidate, keys[0])),
            2 => typeof(GenericGqlMutationWithComplexKey<,,,,,>)
                .MakeGenericType(
                    candidate,
                    genericRequestType,
                    genericResponseType,
                    genericFilterType,
                    GetPropertyTypeOrThrowException(candidate, keys[0]),
                    GetPropertyTypeOrThrowException(candidate, keys[1])),
            3 => typeof(GenericGqlMutationWithComplexKey<,,,,,,>)
                .MakeGenericType(
                    candidate,
                    genericRequestType,
                    genericResponseType,
                    genericFilterType,
                    GetPropertyTypeOrThrowException(candidate, keys[0]),
                    GetPropertyTypeOrThrowException(candidate, keys[1]),
                    GetPropertyTypeOrThrowException(candidate, keys[2])),
            4 => typeof(GenericGqlMutationWithComplexKey<,,,,,,,>)
                .MakeGenericType(
                    candidate,
                    genericFilterType,
                    GetPropertyTypeOrThrowException(candidate, keys[0]),
                    GetPropertyTypeOrThrowException(candidate, keys[1]),
                    GetPropertyTypeOrThrowException(candidate, keys[2]),
                    GetPropertyTypeOrThrowException(candidate, keys[3])),
            5 => typeof(GenericGqlMutationWithComplexKey<,,,,,,,,>)
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
            6 => typeof(GenericGqlMutationWithComplexKey<,,,,,,,,,>)
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
            7 => typeof(GenericGqlMutationWithComplexKey<,,,,,,,,,,>)
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
            8 => typeof(GenericGqlMutationWithComplexKey<,,,,,,,,,,,>)
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
                       Constants.UnableToBuildAGenericGraphQlMutationForTypeThePropertyHasNotBeenFound,
                       candidate.FullName,
                       key));
    }
}