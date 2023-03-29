using System.Reflection;
using System.Reflection.Emit;
using AutoBackend.Sdk.Attributes;
using AutoBackend.Sdk.Exceptions;
using AutoBackend.Sdk.Extensions;
using AutoBackend.Sdk.Filters.Infrastructure;

namespace AutoBackend.Sdk.GraphQL.Infrastructure;

internal static class GenericGqlQueryTypeBuilder
{
    internal const string AssemblyName = "AutoBackend.Sdk.Runtime.GqlQueries";
    private const string GqlQueriesAssemblyModuleName = "GenericGqlQueries";

    internal static Type Build(params Assembly[] assemblies)
    {
        var assemblyBuilder = AssemblyBuilder
            .DefineDynamicAssembly(
                new AssemblyName(AssemblyName),
                AssemblyBuilderAccess.Run);

        var moduleBuilder = assemblyBuilder
            .DefineDynamicModule(GqlQueriesAssemblyModuleName);

        var queryTypeName = "GenericGqlQuery";
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
                var propertyParentType = typeof(GenericGqlQuery<,>)
                    .MakeGenericType(
                        candidate,
                        GenericFilterTypeBuilder.Build(candidate));

                MakeForCandidate(
                    moduleBuilder,
                    queryTypeBuilder,
                    candidate,
                    propertyParentType);
            }
        }

        queryTypeBuilder.SetGraphQLNameAttribute(queryTypeName);
        var queryType = queryTypeBuilder.CreateType();

        return queryType;
    }

    private static void MakeForCandidate(
        ModuleBuilder moduleBuilder,
        TypeBuilder queryTypeBuilder,
        Type candidate,
        Type propertyTypeParent)
    {
        var propertyTypeName = $"{candidate.Name}_GenericGqlQuery";
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

        propertyBuilder.SetGraphQLNameAttribute(candidate.Name);
    }
}