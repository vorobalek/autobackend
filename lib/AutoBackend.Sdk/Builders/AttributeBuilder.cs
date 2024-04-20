using System.Reflection;
using System.Reflection.Emit;
using AutoBackend.Sdk.Exceptions.Reflection;

namespace AutoBackend.Sdk.Builders;

internal static class AttributeBuilder
{
    internal static CustomAttributeBuilder Create<TAttribute>(
        IReadOnlyDictionary<string, object?>? properties = null,
        IReadOnlyDictionary<string, object?>? fields = null,
        params object[] args)
        where TAttribute : Attribute
    {
        var constructorParameters = args.Select(arg => arg.GetType()).ToArray();
        var constructorInfo = typeof(TAttribute).GetConstructor(constructorParameters)
                              ?? throw new NotFoundReflectionException(
                                  string.Format(
                                      Constants.UnableToFindASuitableConstructorForTheType,
                                      typeof(TAttribute).Name));

        var nameProperties = new List<PropertyInfo>();
        var propertyValues = new List<object?>();
        
        if (properties is not null)
        {
            foreach (var (name, value) in properties)
            {
                var propertyInfo = typeof(TAttribute).GetProperty(name);
                if (propertyInfo is null)
                    throw new NotFoundReflectionException(
                        string.Format(
                            Constants.UnableToFindAPropertyWithNameInObject,
                            name,
                            typeof(TAttribute).Name));
                
                nameProperties.Add(propertyInfo);
                propertyValues.Add(value);
            }
        }

        var namedFields = new List<FieldInfo>();
        var fieldValues = new List<object?>();
        
        if (fields is not null)
        {
            foreach (var (name, value) in fields)
            {
                var fieldInfo = typeof(TAttribute).GetField(name);
                if (fieldInfo is null)
                    throw new NotFoundReflectionException(
                        string.Format(
                            Constants.UnableToFindAFieldWithNameInObject,
                            name,
                            typeof(TAttribute).Name));
                
                namedFields.Add(fieldInfo);
                fieldValues.Add(value);
            }
        }

        var attributeBuilder = new CustomAttributeBuilder(
            constructorInfo,
            args,
            nameProperties.ToArray(),
            propertyValues.ToArray(),
            namedFields.ToArray(),
            fieldValues.ToArray());

        return attributeBuilder;
    }
}