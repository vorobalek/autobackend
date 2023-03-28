using System.Collections.Concurrent;
using System.Reflection;
using System.Reflection.Emit;
using AutoBackend.Sdk.Attributes;

namespace AutoBackend.Sdk.Filters.Infrastructure;

internal static class GenericFilterTypeBuilder
{
    private const string FiltersAssemblyModuleName = "Main";

    private static readonly ModuleBuilder ModuleBuilder = AssemblyBuilder
        .DefineDynamicAssembly(
            new AssemblyName(Guid.NewGuid().ToString()),
            AssemblyBuilderAccess.Run)
        .DefineDynamicModule(FiltersAssemblyModuleName);

    private static readonly ConcurrentDictionary<Type, Type?> FiltersMap = new();

    internal static Type? TryBuild(Type type)
    {
        if (FiltersMap.TryGetValue(type, out var filterType))
            return filterType;

        var candidateProperties = type
            .GetProperties()
            .Where(p => p.GetCustomAttributes<GenericFilterAttribute>().Any())
            .ToArray();

        if (candidateProperties.Any())
        {
            var filterTypeName = $"{type.Name}GenericFilters";

            var filterTypeBuilder = ModuleBuilder.DefineType(
                filterTypeName,
                TypeAttributes.Public |
                TypeAttributes.Class |
                TypeAttributes.AutoClass |
                TypeAttributes.AnsiClass |
                TypeAttributes.BeforeFieldInit |
                TypeAttributes.AutoLayout,
                null);

            filterTypeBuilder.DefineDefaultConstructor(
                MethodAttributes.Public |
                MethodAttributes.SpecialName |
                MethodAttributes.RTSpecialName);

            foreach (var candidateProperty in candidateProperties)
            {
                var propertyName = candidateProperty.Name;

                var nullableCandidatePropertyType = candidateProperty.PropertyType.IsValueType
                    ? Nullable.GetUnderlyingType(candidateProperty.PropertyType) is { } underlyingType
                        ? typeof(Nullable<>).MakeGenericType(underlyingType)
                        : typeof(Nullable<>).MakeGenericType(candidateProperty.PropertyType)
                    : candidateProperty.PropertyType;
                var propertyType =
                    typeof(GenericFilter<,>).MakeGenericType(candidateProperty.PropertyType,
                        nullableCandidatePropertyType);

                var fieldBuilder =
                    filterTypeBuilder.DefineField("_" + propertyName, propertyType, FieldAttributes.Private);
                var propertyBuilder =
                    filterTypeBuilder.DefineProperty(propertyName, PropertyAttributes.HasDefault, propertyType, null);

                var getMethod = filterTypeBuilder.DefineMethod("get_" + propertyName,
                    MethodAttributes.Public |
                    MethodAttributes.SpecialName |
                    MethodAttributes.HideBySig, propertyType, Type.EmptyTypes);

                var getMethodIl = getMethod.GetILGenerator();
                getMethodIl.Emit(OpCodes.Ldarg_0);
                getMethodIl.Emit(OpCodes.Ldfld, fieldBuilder);
                getMethodIl.Emit(OpCodes.Ret);

                var setMethod = filterTypeBuilder.DefineMethod("set_" + propertyName,
                    MethodAttributes.Public |
                    MethodAttributes.SpecialName |
                    MethodAttributes.HideBySig,
                    null, new[] { propertyType });

                var setMethodIl = setMethod.GetILGenerator();
                var modifyProperty = setMethodIl.DefineLabel();
                var exitSet = setMethodIl.DefineLabel();

                setMethodIl.MarkLabel(modifyProperty);
                setMethodIl.Emit(OpCodes.Ldarg_0);
                setMethodIl.Emit(OpCodes.Ldarg_1);
                setMethodIl.Emit(OpCodes.Stfld, fieldBuilder);
                setMethodIl.Emit(OpCodes.Nop);
                setMethodIl.MarkLabel(exitSet);
                setMethodIl.Emit(OpCodes.Ret);

                propertyBuilder.SetGetMethod(getMethod);
                propertyBuilder.SetSetMethod(setMethod);
            }

            filterType = filterTypeBuilder.CreateType();
        }

        FiltersMap.TryAdd(type, filterType);
        return filterType;
    }
}