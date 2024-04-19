using System.Linq.Expressions;
using AutoBackend.Sdk.Exceptions.Reflection;
using AutoBackend.Sdk.Models;

namespace AutoBackend.Sdk.Data.Mappers;

internal class GenericRequestMapper<TEntity, TRequest>
    : IGenericRequestMapper<TEntity, TRequest>
    where TEntity : class, new()
    where TRequest : class, IGenericRequest
{
    public TEntity ToEntity(TRequest model)
    {
        var expr = MapExpr();
        var func = expr.Compile();
        return func(model);
    }

    private Expression<Func<TRequest, TEntity>> MapExpr()
    {
        var parameter = Expression.Parameter(typeof(TRequest));

        var properties = typeof(TRequest).GetProperties();

        var bindings = new List<MemberBinding>();
        foreach (var sourceProperty in properties)
        {
            var destinationProperty = typeof(TEntity).GetProperty(sourceProperty.Name)
                                      ?? throw new InheritanceReflectionException();

            var propertyExpr = Expression.Property(parameter, sourceProperty);

            if (!sourceProperty.PropertyType.IsAssignableTo(typeof(IGenericRequest)))
            {
                bindings.Add(Expression.Bind(destinationProperty, propertyExpr));
                continue;
            }

            var mapMethodInfo = GetType().GetMethod(nameof(ToEntity)) ?? throw new InheritanceReflectionException();

            bindings.Add(Expression.Bind(
                destinationProperty,
                Expression.Condition(
                    Expression.NotEqual(
                        propertyExpr,
                        Expression.Constant(null)),
                    Expression.Call(Expression.Constant(this), mapMethodInfo, propertyExpr),
                    Expression.Default(destinationProperty.PropertyType))));
        }

        return Expression.Lambda<Func<TRequest, TEntity>>(
            Expression.MemberInit(
                Expression.New(
                    typeof(TEntity)),
                bindings.ToArray()),
            parameter);
    }
}