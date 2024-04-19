using System.Linq.Expressions;
using AutoBackend.Sdk.Exceptions.Reflection;
using AutoBackend.Sdk.Models;

namespace AutoBackend.Sdk.Data.Mappers;

internal class GenericResponseMapper<TEntity, TModel> : IGenericResponseMapper<TEntity, TModel>
    where TEntity : class
    where TModel : class, IGenericResponse, new()
{
    public TModel ToModel(TEntity entity)
    {
        var expr = MapExpr();
        var func = expr.Compile();
        return func(entity);
    }

    public IEnumerable<TModel> ToModel(IEnumerable<TEntity> entities)
    {
        foreach (var entity in entities) yield return ToModel(entity);
    }

    private Expression<Func<TEntity, TModel>> MapExpr()
    {
        var parameter = Expression.Parameter(typeof(TEntity));

        var properties = typeof(TModel).GetProperties();

        var bindings = new List<MemberBinding>();
        foreach (var destinationProperty in properties)
        {
            var sourceProperty = typeof(TEntity).GetProperty(destinationProperty.Name)
                                 ?? throw new InheritanceReflectionException();

            var propertyExpr = Expression.Property(parameter, sourceProperty);

            if (!destinationProperty.PropertyType.IsAssignableTo(typeof(IGenericResponse)))
            {
                bindings.Add(Expression.Bind(destinationProperty, propertyExpr));
                continue;
            }

            var mapMethodInfo = GetType().GetMethod(nameof(ToModel)) ?? throw new InheritanceReflectionException();

            bindings.Add(Expression.Bind(
                destinationProperty,
                Expression.Condition(
                    Expression.NotEqual(
                        propertyExpr,
                        Expression.Constant(null)),
                    Expression.Call(Expression.Constant(this), mapMethodInfo, propertyExpr),
                    Expression.Default(destinationProperty.PropertyType))));
        }

        return Expression.Lambda<Func<TEntity, TModel>>(
            Expression.MemberInit(
                Expression.New(
                    typeof(TModel)),
                bindings.ToArray()),
            parameter);
    }
}