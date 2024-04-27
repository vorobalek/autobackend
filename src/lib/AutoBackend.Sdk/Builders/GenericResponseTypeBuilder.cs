using System.Collections.Concurrent;
using System.Reflection;
using System.Reflection.Emit;
using AutoBackend.Sdk.Attributes;
using AutoBackend.Sdk.Exceptions.Reflection;
using AutoBackend.Sdk.Extensions;
using AutoBackend.Sdk.Models;

namespace AutoBackend.Sdk.Builders;

internal static class GenericResponseTypeBuilder
{
    private static readonly ModuleBuilder ModuleBuilder =
        AssemblyBuilder
            .DefineDynamicAssembly(
                new AssemblyName(Constants.GenericResponsesAssemblyName),
                AssemblyBuilderAccess.Run)
            .DefineDynamicModule(Constants.GenericResponsesModuleName);

    private static readonly ConcurrentDictionary<Type, Type> ResponsesMap = new();

    internal static Type BuildForCandidate(Type candidate)
    {
        if (candidate.IsPrimitiveModelType())
            return candidate;

        if (candidate.IsCollection())
            return typeof(ICollection<>)
                .MakeGenericType(
                    BuildForCandidate(
                        candidate.GetCollectionType()
                        ?? throw new NotFoundReflectionException()));

        if (ResponsesMap.TryGetValue(candidate, out var responseType))
            return responseType;

        var responseTypeName = string.Format(Constants.GenericResponseTypeName, candidate.Name);
        var responseTypeBuilder = ModuleBuilder
            .DefineType(
                responseTypeName,
                TypeAttributes.Public |
                TypeAttributes.Class |
                TypeAttributes.AutoClass |
                TypeAttributes.AnsiClass |
                TypeAttributes.BeforeFieldInit |
                TypeAttributes.AutoLayout,
                typeof(GenericResponse));

        ResponsesMap.AddOrUpdate(
            candidate,
            _ => responseTypeBuilder,
            (_, _) => responseTypeBuilder);

        responseTypeBuilder.DefineDefaultConstructor(
            MethodAttributes.Public |
            MethodAttributes.SpecialName |
            MethodAttributes.RTSpecialName);

        var candidatePropertyNames = candidate
            .GetCustomAttribute<GenericResponseAttribute>()
            ?.Properties;

        var candidatePropertiesExpression = ExpressionBuilder
            .True<PropertyInfo>();

        if (candidatePropertyNames is not null)
            candidatePropertiesExpression = candidatePropertiesExpression
                .And(p => candidatePropertyNames.Contains(p.Name));

        var candidateProperties = candidate
            .GetProperties()
            .Where(candidatePropertiesExpression.Compile())
            .ToArray();

        if (candidateProperties.Any())
            foreach (var candidateProperty in candidateProperties)
            {
                var propertyName = candidateProperty.Name;
                var propertyType = BuildForCandidate(candidateProperty.PropertyType);

                var fieldModelBuilder = responseTypeBuilder
                    .DefineField(
                        string.Format(
                            Constants.PropertyBackingFieldName,
                            propertyName),
                        propertyType,
                        FieldAttributes.Private);
                var propertyBuilder = responseTypeBuilder
                    .DefineProperty(
                        propertyName,
                        PropertyAttributes.HasDefault,
                        propertyType,
                        null);

                var getMethod = responseTypeBuilder
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

                var setMethod = responseTypeBuilder
                    .DefineMethod(
                        string.Format(
                            Constants.PropertySetterName,
                            propertyName),
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

                var propertyNameSnakeCase = propertyName.ToCamelCase();
                propertyBuilder.SetJsonPropertyAttribute(propertyNameSnakeCase);
                propertyBuilder.SetJsonPropertyNameAttribute(propertyNameSnakeCase);
                propertyBuilder.SetGraphQLNameAttribute(propertyNameSnakeCase);
                propertyBuilder.SetBindPropertyAttribute(propertyNameSnakeCase);
            }

        responseTypeBuilder.SetGraphQLNameAttribute(responseTypeName);
        responseType = responseTypeBuilder.CreateType();

        ResponsesMap.AddOrUpdate(
            candidate,
            _ => responseType,
            (_, _) => responseType);
        return responseType;
    }
}