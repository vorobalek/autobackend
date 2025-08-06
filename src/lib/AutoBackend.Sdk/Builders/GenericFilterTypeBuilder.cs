using System.Collections.Concurrent;
using System.Reflection;
using System.Reflection.Emit;
using AutoBackend.Sdk.Attributes;
using AutoBackend.Sdk.Extensions;
using AutoBackend.Sdk.Filters;

namespace AutoBackend.Sdk.Builders;

internal static class GenericFilterTypeBuilder
{
    private static readonly ModuleBuilder ModuleBuilder =
        AssemblyBuilder
            .DefineDynamicAssembly(
                new AssemblyName(Constants.GenericFiltersAssemblyName),
                AssemblyBuilderAccess.Run)
            .DefineDynamicModule(Constants.GenericFiltersModuleName);

    private static readonly ConcurrentDictionary<Type, Type> FiltersMap = new();

    internal static Type BuildForCandidate(Type candidate)
    {
        if (FiltersMap.TryGetValue(candidate, out var filterType))
            return filterType;

        var filterTypeName = string.Format(Constants.GenericFilterTypeName, candidate.Name);
        var filterTypeBuilder = ModuleBuilder
            .DefineType(
                filterTypeName,
                TypeAttributes.Public |
                TypeAttributes.Class |
                TypeAttributes.AutoClass |
                TypeAttributes.AnsiClass |
                TypeAttributes.BeforeFieldInit |
                TypeAttributes.AutoLayout,
                typeof(GenericFilter));

        filterTypeBuilder.DefineDefaultConstructor(
            MethodAttributes.Public |
            MethodAttributes.SpecialName |
            MethodAttributes.RTSpecialName);

        var candidateProperties = candidate
            .GetProperties()
            .Where(p => p.GetCustomAttributes<GenericFilterAttribute>().Any())
            .ToArray();

        if (candidateProperties.Any())
            foreach (var candidateProperty in candidateProperties)
            {
                var propertyName = candidateProperty.Name;

                var nullableCandidatePropertyType = candidateProperty.PropertyType.IsValueType
                    ? Nullable.GetUnderlyingType(candidateProperty.PropertyType) is { } underlyingType
                        ? typeof(Nullable<>).MakeGenericType(underlyingType)
                        : typeof(Nullable<>).MakeGenericType(candidateProperty.PropertyType)
                    : candidateProperty.PropertyType;

                var propertyTypeName = string.Format(
                    Constants.GenericFilterPropertyTypeName,
                    candidate.Name,
                    propertyName);

                var propertyTypeBuilder = ModuleBuilder
                    .DefineType(
                        propertyTypeName,
                        TypeAttributes.Public |
                        TypeAttributes.Class |
                        TypeAttributes.AutoClass |
                        TypeAttributes.AnsiClass |
                        TypeAttributes.BeforeFieldInit |
                        TypeAttributes.AutoLayout,
                        typeof(GenericPropertyFilter<,>).MakeGenericType(candidateProperty.PropertyType,
                            nullableCandidatePropertyType));

                propertyTypeBuilder
                    .DefineDefaultConstructor(
                        MethodAttributes.Public |
                        MethodAttributes.SpecialName |
                        MethodAttributes.RTSpecialName);

                propertyTypeBuilder.SetGraphQLNameAttribute(propertyTypeName);

                var propertyType = propertyTypeBuilder.CreateType();

                var fieldModelBuilder = filterTypeBuilder
                    .DefineField(
                        string.Format(
                            Constants.PropertyBackingFieldName,
                            propertyName),
                        propertyType,
                        FieldAttributes.Private);
                var propertyBuilder = filterTypeBuilder
                    .DefineProperty(
                        propertyName,
                        PropertyAttributes.HasDefault,
                        propertyType,
                        null);

                var getMethod = filterTypeBuilder
                    .DefineMethod(
                        string.Format(
                            Constants.PropertyGetterName,
                            propertyName),
                        MethodAttributes.Public |
                        MethodAttributes.SpecialName |
                        MethodAttributes.HideBySig, propertyType, Type.EmptyTypes);

                var getMethodIl = getMethod.GetILGenerator();
                getMethodIl.Emit(OpCodes.Ldarg_0);
                getMethodIl.Emit(OpCodes.Ldfld, fieldModelBuilder);
                getMethodIl.Emit(OpCodes.Ret);

                var setMethod = filterTypeBuilder
                    .DefineMethod(
                        string.Format(
                            Constants.PropertySetterName,
                            propertyName),
                        MethodAttributes.Public |
                        MethodAttributes.SpecialName |
                        MethodAttributes.HideBySig,
                        null, [propertyType]);

                var setMethodIl = setMethod.GetILGenerator();
                var modifyProperty = setMethodIl.DefineLabel();
                var exitSet = setMethodIl.DefineLabel();

                setMethodIl.MarkLabel(modifyProperty);
                setMethodIl.Emit(OpCodes.Ldarg_0);
                setMethodIl.Emit(OpCodes.Ldarg_1);
                setMethodIl.Emit(OpCodes.Stfld, fieldModelBuilder);
                setMethodIl.Emit(OpCodes.Nop);
                setMethodIl.MarkLabel(exitSet);
                setMethodIl.Emit(OpCodes.Ret);

                propertyBuilder.SetGetMethod(getMethod);
                propertyBuilder.SetSetMethod(setMethod);

                var propertyNameSnakeCase = propertyName.ToCamelCase();
                propertyBuilder.SetJsonPropertyAttribute(propertyNameSnakeCase);
                propertyBuilder.SetJsonPropertyNameAttribute(propertyNameSnakeCase);
                propertyBuilder.SetGraphQLNameAttribute(propertyNameSnakeCase);
                propertyBuilder.SetBindPropertyAttribute(propertyNameSnakeCase);
            }

        filterTypeBuilder.SetGraphQLNameAttribute(filterTypeName);
        filterType = filterTypeBuilder.CreateType();
        FiltersMap.TryAdd(candidate, filterType);
        return filterType;
    }
}