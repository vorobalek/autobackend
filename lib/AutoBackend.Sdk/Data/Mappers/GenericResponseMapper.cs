using System.Linq.Expressions;
using AutoBackend.Sdk.Exceptions.Data;
using AutoBackend.Sdk.Exceptions.Reflection;
using AutoBackend.Sdk.Extensions;
using AutoBackend.Sdk.Models;

namespace AutoBackend.Sdk.Data.Mappers;

internal class GenericResponseMapper(IMapperExpressionsCache cache) : IGenericResponseMapper
{
    public TResponse? ToModel<TEntity, TResponse>(TEntity? entity)
        where TEntity : class
        where TResponse : class, IGenericResponse, new()
    {
        if (entity is null) return null;

        var func = cache.GetOrAddAndCompile(MapExpr<TEntity, TResponse>());
        return func(entity);
    }

    public IEnumerable<TResponse>? ToModelEnumerable<TEntity, TResponse>(IEnumerable<TEntity>? entities)
        where TEntity : class
        where TResponse : class, IGenericResponse, new()
    {
        return entities?.Select(entity => 
            ToModel<TEntity, TResponse>(entity) 
            ?? throw new InconsistentDataException()) 
               ?? Array.Empty<TResponse>();
    }

    private Expression<Func<TEntity, TModel>> MapExpr<TEntity, TModel>()
        where TEntity : class
        where TModel : class, IGenericResponse, new()
    {
        var parameter = Expression.Parameter(typeof(TEntity));

        var properties = typeof(TModel).GetProperties();

        var bindings = new List<MemberBinding>();
        foreach (var destinationProperty in properties)
        {
            var sourceProperty = typeof(TEntity).GetProperty(destinationProperty.Name)
                                 ?? throw new InheritanceReflectionException();

            var sourceExpr = Expression.Property(parameter, sourceProperty);

            if (destinationProperty.PropertyType.IsAssignableTo(typeof(IGenericResponse)))
            {
                var mapMethodInfo = GetType().GetMethod(nameof(ToModel)) 
                                    ?? throw new InheritanceReflectionException();

                var genericMapMethodInfo = mapMethodInfo.MakeGenericMethod(
                    sourceProperty.PropertyType, 
                    destinationProperty.PropertyType);

                bindings.Add(Expression.Bind(
                    destinationProperty,
                    Expression.Call(
                        Expression.Constant(this), 
                        genericMapMethodInfo, 
                        sourceExpr)));
                
                continue;
            }

            if (destinationProperty.PropertyType.IsEnumerable())
            {
                if (sourceProperty.PropertyType.GetEnumerableType() is not { } sourceEnumType ||
                    destinationProperty.PropertyType.GetEnumerableType() is not { } destinationEnumType)
                    throw new NotFoundReflectionException();
                
                if (destinationEnumType.IsAssignableTo(typeof(IGenericResponse)))
                {
                    var mapMethodInfo = GetType().GetMethod(nameof(ToModelEnumerable))
                                        ?? throw new NotFoundReflectionException();

                    var genericMapMethodInfo = mapMethodInfo.MakeGenericMethod(
                        sourceEnumType,
                        destinationEnumType);

                    bindings.Add(Expression.Bind(
                        destinationProperty,
                        Expression.Call(
                            Expression.Constant(this),
                            genericMapMethodInfo,
                            sourceExpr)));

                    continue;
                }
            }

            bindings.Add(Expression.Bind(destinationProperty, sourceExpr));
        }

        return Expression.Lambda<Func<TEntity, TModel>>(
            Expression.MemberInit(
                Expression.New(
                    typeof(TModel)),
                bindings.ToArray()),
            parameter);
    }
}