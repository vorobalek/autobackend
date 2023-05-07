using System.Reflection;
using System.Reflection.Emit;
using AutoBackend.Sdk.Attributes;
using AutoBackend.Sdk.Exceptions.Reflection;
using AutoBackend.Sdk.Extensions;
using AutoBackend.Sdk.GraphQL.Queries;

namespace AutoBackend.Sdk.Helpers;

internal static class GenericGqlQueryTypeBuilder
{
    private static readonly ModuleBuilder ModuleBuilder =
        AssemblyBuilder
            .DefineDynamicAssembly(
                new AssemblyName(Constants.GenericGqlQueriesAssemblyName),
                AssemblyBuilderAccess.Run)
            .DefineDynamicModule(Constants.GenericGqlQueriesModuleName);

    internal static Type Build(params Assembly[] assemblies)
    {
        var queryTypeName = Constants.GenericGqlQueryTypeName;
        var queryTypeBuilder = ModuleBuilder.DefineType(
            queryTypeName,
            TypeAttributes.Public |
            TypeAttributes.Class |
            TypeAttributes.AutoClass |
            TypeAttributes.AnsiClass |
            TypeAttributes.BeforeFieldInit |
            TypeAttributes.AutoLayout,
            null);

        queryTypeBuilder.DefineDefaultConstructor(
            MethodAttributes.Public |
            MethodAttributes.HideBySig |
            MethodAttributes.SpecialName);

        foreach (var currentAssembly in assemblies)
        {
            var candidates = currentAssembly.GetExportedTypes();
            foreach (var candidate in candidates
                         .Where(candidate => candidate
                             .GetCustomAttribute<GenericGqlQueryAttribute>() is not null))
                BuildProperty(
                    queryTypeBuilder,
                    candidate);
        }

        queryTypeBuilder.SetGraphQLNameAttribute(queryTypeName);
        var queryType = queryTypeBuilder.CreateType();

        return queryType;
    }

    private static void BuildProperty(
        TypeBuilder queryTypeBuilder,
        Type propertyCandidateType)
    {
        var propertyTypeParent = GetPropertyTypeForCandidate(propertyCandidateType);

        var propertyTypeName = string.Format(
            Constants.GenericGqlQueryPropertyTypeName,
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
        var propertyBuilder = queryTypeBuilder
            .DefineProperty(
                propertyName,
                PropertyAttributes.None,
                propertyType,
                null);

        var getMethod = queryTypeBuilder
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

        propertyBuilder.SetGraphQLNameAttribute(propertyCandidateType.Name);
    }

    private static Type GetPropertyTypeForCandidate(Type candidate)
    {
        if (candidate.GetCustomAttribute<GenericEntityAttribute>() is not { Keys: { } } genericEntityAttribute)
            throw new NotFoundReflectionException(
                string.Format(
                    Constants.AGenericGraphQlQueryCanBeGeneratedOnlyForTypesMarkedWith,
                    nameof(GenericEntityAttribute),
                    candidate.Name));

        var genericFilterType = GenericFilterTypeBuilder.BuildForCandidate(candidate);
        var keys = genericEntityAttribute.Keys;
        return keys.Length switch
        {
            0 => typeof(GenericGqlQueryWithNoKey<,>)
                .MakeGenericType(
                    candidate,
                    genericFilterType),
            1 => typeof(GenericGqlQueryWithPrimaryKey<,,>)
                .MakeGenericType(
                    candidate,
                    genericFilterType,
                    GetPropertyTypeOrThrowException(candidate, keys[0])),
            2 => typeof(GenericGqlQueryWithComplexKey<,,,>)
                .MakeGenericType(
                    candidate,
                    genericFilterType,
                    GetPropertyTypeOrThrowException(candidate, keys[0]),
                    GetPropertyTypeOrThrowException(candidate, keys[1])),
            3 => typeof(GenericGqlQueryWithComplexKey<,,,,>)
                .MakeGenericType(
                    candidate,
                    genericFilterType,
                    GetPropertyTypeOrThrowException(candidate, keys[0]),
                    GetPropertyTypeOrThrowException(candidate, keys[1]),
                    GetPropertyTypeOrThrowException(candidate, keys[2])),
            4 => typeof(GenericGqlQueryWithComplexKey<,,,,,>)
                .MakeGenericType(
                    candidate,
                    genericFilterType,
                    GetPropertyTypeOrThrowException(candidate, keys[0]),
                    GetPropertyTypeOrThrowException(candidate, keys[1]),
                    GetPropertyTypeOrThrowException(candidate, keys[2]),
                    GetPropertyTypeOrThrowException(candidate, keys[3])),
            5 => typeof(GenericGqlQueryWithComplexKey<,,,,,,>)
                .MakeGenericType(
                    candidate,
                    genericFilterType,
                    GetPropertyTypeOrThrowException(candidate, keys[0]),
                    GetPropertyTypeOrThrowException(candidate, keys[1]),
                    GetPropertyTypeOrThrowException(candidate, keys[2]),
                    GetPropertyTypeOrThrowException(candidate, keys[3]),
                    GetPropertyTypeOrThrowException(candidate, keys[4])),
            6 => typeof(GenericGqlQueryWithComplexKey<,,,,,,,>)
                .MakeGenericType(
                    candidate,
                    genericFilterType,
                    GetPropertyTypeOrThrowException(candidate, keys[0]),
                    GetPropertyTypeOrThrowException(candidate, keys[1]),
                    GetPropertyTypeOrThrowException(candidate, keys[2]),
                    GetPropertyTypeOrThrowException(candidate, keys[3]),
                    GetPropertyTypeOrThrowException(candidate, keys[4]),
                    GetPropertyTypeOrThrowException(candidate, keys[5])),
            7 => typeof(GenericGqlQueryWithComplexKey<,,,,,,,,>)
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
            8 => typeof(GenericGqlQueryWithComplexKey<,,,,,,,,,>)
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
               ?? throw new NotFoundReflectionException(
                   string.Format(
                       Constants.UnableToBuildAGenericGraphQlQueryForTypeThePropertyHasNotBeenFound,
                       candidate.FullName,
                       key));
    }
}