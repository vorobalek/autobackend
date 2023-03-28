using System.Reflection;
using System.Reflection.Emit;
using AutoBackend.Sdk.Attributes;
using AutoBackend.Sdk.Exceptions;
using AutoBackend.Sdk.Extensions;
using AutoBackend.Sdk.Filters.Infrastructure;

namespace AutoBackend.Sdk.GraphQL.Infrastructure;

internal static class GenericGqlQueryTypeBuilder
{
    private const string GqlQueriesAssemblyModuleName = "GqlQueries";

    internal static Type Build(params Assembly[] assemblies)
    {
        var assemblyBuilder = AssemblyBuilder
            .DefineDynamicAssembly(
                new AssemblyName("AutoBackend.Sdk.Runtime.GqlQueries"),
                AssemblyBuilderAccess.Run);

        var moduleBuilder = assemblyBuilder
            .DefineDynamicModule(GqlQueriesAssemblyModuleName);

        var queryTypeName = "GenericGqlQueryRoot";
        var queryTypeBuilder = moduleBuilder.DefineType(
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
            {
                MakeForCandidate(moduleBuilder, queryTypeBuilder, candidate);
                
                if (GenericFilterModelTypeBuilder.TryBuild(candidate) is { } genericFilterType)
                    MakeWithFilterForCandidate(moduleBuilder, queryTypeBuilder, candidate, genericFilterType);
            }
        }

        queryTypeBuilder.SetGraphQLNameAttribute(queryTypeName);
        var queryType = queryTypeBuilder.CreateType();

        return queryType;
    }

    private static void MakeWithFilterForCandidate(ModuleBuilder moduleBuilder, TypeBuilder queryTypeBuilder, Type candidate, Type genericFilterType)
    {
        MakeForCandidateInternal(
            moduleBuilder,
            queryTypeBuilder,
            candidate,
            $"Generic_{candidate.Name}GqlQuery_WithFilter",
            typeof(GenericGqlQueryWithFilter<,>)
                .MakeGenericType(
                    candidate,
                    genericFilterType),
            $"{candidate.Name}WithFilter");
    }

    private static void MakeForCandidate(ModuleBuilder moduleBuilder, TypeBuilder queryTypeBuilder, Type candidate)
    {
        MakeForCandidateInternal(
            moduleBuilder,
            queryTypeBuilder,
            candidate,
            $"Generic_{candidate.Name}GqlQuery",
            typeof(GenericGqlQuery<>)
                .MakeGenericType(
                    candidate),
            candidate.Name);
    }

    private static void MakeForCandidateInternal(
        ModuleBuilder moduleBuilder,
        TypeBuilder queryTypeBuilder,
        Type candidate,
        string propertyTypeName, 
        Type propertyTypeParent,
        string propertyGqlName)
    {
        var propertyTypeBuilder = moduleBuilder.DefineType(
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

        var propertyName = candidate.Name;
        var propertyBuilder =
            queryTypeBuilder.DefineProperty(propertyName, PropertyAttributes.None, propertyType, null);

        var getMethod = queryTypeBuilder.DefineMethod("get_" + propertyName,
            MethodAttributes.Public |
            MethodAttributes.SpecialName |
            MethodAttributes.HideBySig, propertyType, Type.EmptyTypes);

        var constructorInfo = propertyType.GetConstructor(Array.Empty<Type>());

        var getMethodIl = getMethod.GetILGenerator();
        getMethodIl.Emit(OpCodes.Newobj, constructorInfo ?? throw new AutoBackendException(
            $"Unable to determine suitable constructor for type {propertyType.Name}"));
        getMethodIl.Emit(OpCodes.Ret);

        propertyBuilder.SetGetMethod(getMethod);

        propertyBuilder.SetGraphQLNameAttribute(propertyGqlName);
    }
}