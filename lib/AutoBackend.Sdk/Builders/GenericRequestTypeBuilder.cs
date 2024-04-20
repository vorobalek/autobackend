using System.Collections.Concurrent;
using System.Reflection;
using System.Reflection.Emit;
using AutoBackend.Sdk.Attributes;
using AutoBackend.Sdk.Exceptions.Reflection;
using AutoBackend.Sdk.Extensions;
using AutoBackend.Sdk.Models;

namespace AutoBackend.Sdk.Builders;

internal static class GenericRequestTypeBuilder
{
    private static readonly ModuleBuilder ModuleBuilder =
        AssemblyBuilder
            .DefineDynamicAssembly(
                new AssemblyName(Constants.GenericRequestsAssemblyName),
                AssemblyBuilderAccess.Run)
            .DefineDynamicModule(Constants.GenericRequestsModuleName);

    private static readonly ConcurrentDictionary<Type, Type> RequestsMap = new();

    internal static Type BuildForCandidate(Type candidate)
    {
        if (candidate.IsPrimitiveModelType())
            return candidate;

        if (candidate.IsEnumerable())
            return typeof(IEnumerable<>)
                .MakeGenericType(
                    BuildForCandidate(
                        candidate.GetEnumerableType()
                        ?? throw new NotFoundReflectionException()));

        if (RequestsMap.TryGetValue(candidate, out var requestType))
            return requestType;

        var requestTypeName = string.Format(Constants.GenericRequestTypeName, candidate.Name);
        var requestTypeBuilder = ModuleBuilder
            .DefineType(
                requestTypeName,
                TypeAttributes.Public |
                TypeAttributes.Class |
                TypeAttributes.AutoClass |
                TypeAttributes.AnsiClass |
                TypeAttributes.BeforeFieldInit |
                TypeAttributes.AutoLayout,
                typeof(GenericRequest));

        RequestsMap.AddOrUpdate(
            candidate,
            _ => requestTypeBuilder,
            (_, _) => requestTypeBuilder);

        requestTypeBuilder.DefineDefaultConstructor(
            MethodAttributes.Public |
            MethodAttributes.SpecialName |
            MethodAttributes.RTSpecialName);

        var candidatePropertyNames = candidate
            .GetCustomAttribute<GenericRequestAttribute>()
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

                var fieldModelBuilder = requestTypeBuilder
                    .DefineField(
                        string.Format(
                            Constants.PropertyBackingFieldName,
                            propertyName),
                        propertyType,
                        FieldAttributes.Private);
                var propertyBuilder = requestTypeBuilder
                    .DefineProperty(
                        propertyName,
                        PropertyAttributes.HasDefault,
                        propertyType,
                        null);

                var getMethod = requestTypeBuilder
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

                var setMethod = requestTypeBuilder
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

        requestTypeBuilder.SetGraphQLNameAttribute(requestTypeName);
        requestType = requestTypeBuilder.CreateType();

        RequestsMap.AddOrUpdate(
            candidate,
            _ => requestTypeBuilder,
            (_, _) => requestType);
        return requestType;
    }
}