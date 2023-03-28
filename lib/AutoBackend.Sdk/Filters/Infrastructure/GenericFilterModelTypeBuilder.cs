using System.Collections.Concurrent;
using System.Reflection;
using System.Reflection.Emit;
using AutoBackend.Sdk.Attributes;

namespace AutoBackend.Sdk.Filters.Infrastructure;

internal static class GenericFilterModelTypeBuilder
{
    private const string GenericFilterModelsAssemblyModuleName = "GenericFilterModels";

    private static readonly ModuleBuilder ModuleBuilder = AssemblyBuilder
        .DefineDynamicAssembly(
            new AssemblyName("AutoBackend.Sdk.Runtime.Filters"),
            AssemblyBuilderAccess.Run)
        .DefineDynamicModule(GenericFilterModelsAssemblyModuleName);

    private static readonly ConcurrentDictionary<Type, Type?> FilterModelsMap = new();

    internal static Type? TryBuild(Type type)
    {
        if (FilterModelsMap.TryGetValue(type, out var filterType))
            return filterType;

        var candidateProperties = type
            .GetProperties()
            .Where(p => p.GetCustomAttributes<GenericFilterAttribute>().Any())
            .ToArray();

        if (candidateProperties.Any())
        {
            var filterModelTypeName = $"Generic_{type.Name}_Filter";

            var filterModelTypeBuilder = ModuleBuilder.DefineType(
                filterModelTypeName,
                TypeAttributes.Public |
                TypeAttributes.Class |
                TypeAttributes.AutoClass |
                TypeAttributes.AnsiClass |
                TypeAttributes.BeforeFieldInit |
                TypeAttributes.AutoLayout,
                null);

            filterModelTypeBuilder.DefineDefaultConstructor(
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

                var propertyTypeName = $"Generic_{type.Name}_Filter_{propertyName}";
                
                var propertyTypeBuilder = ModuleBuilder.DefineType(
                    propertyTypeName,
                    TypeAttributes.Public |
                    TypeAttributes.Class |
                    TypeAttributes.AutoClass |
                    TypeAttributes.AnsiClass |
                    TypeAttributes.BeforeFieldInit |
                    TypeAttributes.AutoLayout,
                    typeof(GenericFilter<,>).MakeGenericType(candidateProperty.PropertyType,
                        nullableCandidatePropertyType));

                propertyTypeBuilder.DefineDefaultConstructor(
                    MethodAttributes.Public |
                    MethodAttributes.SpecialName |
                    MethodAttributes.RTSpecialName);
                
                var propertyType = propertyTypeBuilder.CreateType();

                var fieldModelBuilder =
                    filterModelTypeBuilder.DefineField("_" + propertyName, propertyType, FieldAttributes.Private);
                var propertyBuilder =
                    filterModelTypeBuilder.DefineProperty(propertyName, PropertyAttributes.HasDefault, propertyType, null);

                var getMethod = filterModelTypeBuilder.DefineMethod("get_" + propertyName,
                    MethodAttributes.Public |
                    MethodAttributes.SpecialName |
                    MethodAttributes.HideBySig, propertyType, Type.EmptyTypes);

                var getMethodIl = getMethod.GetILGenerator();
                getMethodIl.Emit(OpCodes.Ldarg_0);
                getMethodIl.Emit(OpCodes.Ldfld, fieldModelBuilder);
                getMethodIl.Emit(OpCodes.Ret);

                var setMethod = filterModelTypeBuilder.DefineMethod("set_" + propertyName,
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
                setMethodIl.Emit(OpCodes.Stfld, fieldModelBuilder);
                setMethodIl.Emit(OpCodes.Nop);
                setMethodIl.MarkLabel(exitSet);
                setMethodIl.Emit(OpCodes.Ret);

                propertyBuilder.SetGetMethod(getMethod);
                propertyBuilder.SetSetMethod(setMethod);
            }

            filterType = filterModelTypeBuilder.CreateType();
        }

        FilterModelsMap.TryAdd(type, filterType);
        return filterType;
    }
}