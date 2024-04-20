using System.Linq.Expressions;
using AutoBackend.Sdk.Exceptions.Reflection;
using AutoBackend.Sdk.Models;

namespace AutoBackend.Sdk.Data.Mappers;

internal class GenericRequestMapper(IMapperExpressionsCache cache) : IGenericRequestMapper
{
    public TEntity ToEntity<TEntity, TRequest>(TRequest model)
        where TEntity : class, new()
        where TRequest : class, IGenericRequest
    {
        var func = cache.GetOrAddAndCompile(MapExpr<TEntity, TRequest>());
        return func(model);
    }

    private Expression<Func<TRequest, TEntity>> MapExpr<TEntity, TRequest>()
        where TEntity : class, new()
        where TRequest : class, IGenericRequest
    {
        var parameter = Expression.Parameter(typeof(TRequest));

        var properties = typeof(TRequest).GetProperties();

        var bindings = new List<MemberBinding>();
        foreach (var sourceProperty in properties)
        {
            var destinationProperty = typeof(TEntity).GetProperty(sourceProperty.Name)
                                      ?? throw new InheritanceReflectionException();

            var sourceExpr = Expression.Property(parameter, sourceProperty);

            if (sourceProperty.PropertyType.IsAssignableTo(typeof(IGenericResponse)))
            {
                var mapMethodInfo = typeof(GenericRequestMapper).GetMethod(nameof(ToEntity)) 
                                    ?? throw new InheritanceReflectionException();

                var genericMapMethodInfo = mapMethodInfo.MakeGenericMethod(
                    destinationProperty.PropertyType, 
                    sourceProperty.PropertyType);

                bindings.Add(Expression.Bind(
                    destinationProperty,
                    Expression.Call(
                        Expression.Constant(this), 
                        genericMapMethodInfo, 
                        sourceExpr)));
                
                continue;
            }

            bindings.Add(Expression.Bind(destinationProperty, sourceExpr));
        }

        return Expression.Lambda<Func<TRequest, TEntity>>(
            Expression.MemberInit(
                Expression.New(
                    typeof(TEntity)),
                bindings.ToArray()),
            parameter);
    }
}