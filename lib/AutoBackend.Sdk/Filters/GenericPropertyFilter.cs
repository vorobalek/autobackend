using System.Collections;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json.Serialization;
using AutoBackend.Sdk.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AutoBackend.Sdk.Filters;

internal record GenericPropertyFilter<TOriginal, TNullable> : IGenericPropertyFilter
{
    [JsonProperty("equal")]
    [JsonPropertyName("equal")]
    [GraphQLName("equal")]
    [BindProperty(Name = "equal")]
    public TNullable? Equal { get; init; }

    [JsonProperty("notEqual")]
    [JsonPropertyName("notEqual")]
    [GraphQLName("notEqual")]
    [BindProperty(Name = "notEqual")]
    public TNullable? NotEqual { get; init; }

    [JsonProperty("greaterThan")]
    [JsonPropertyName("greaterThan")]
    [GraphQLName("greaterThan")]
    [BindProperty(Name = "greaterThan")]
    public TNullable? GreaterThan { get; init; }

    [JsonProperty("greaterThanOrEqual")]
    [JsonPropertyName("greaterThanOrEqual")]
    [GraphQLName("greaterThanOrEqual")]
    [BindProperty(Name = "greaterThanOrEqual")]
    public TNullable? GreaterThanOrEqual { get; init; }

    [JsonProperty("lessThan")]
    [JsonPropertyName("lessThan")]
    [GraphQLName("lessThan")]
    [BindProperty(Name = "lessThan")]
    public TNullable? LessThan { get; init; }

    [JsonProperty("lessThanOrEqual")]
    [JsonPropertyName("lessThanOrEqual")]
    [GraphQLName("lessThanOrEqual")]
    [BindProperty(Name = "lessThanOrEqual")]
    public TNullable? LessThanOrEqual { get; init; }

    [JsonProperty("in")]
    [JsonPropertyName("in")]
    [GraphQLName("in")]
    [BindProperty(Name = "in")]
    public IEnumerable<TOriginal>? In { get; init; }

    [JsonProperty("isNull")]
    [JsonPropertyName("isNull")]
    [GraphQLName("isNull")]
    [BindProperty(Name = "isNull")]
    public bool? IsNull { get; init; }

    object? IGenericPropertyFilter.Equal => Equal;

    public Expression<Func<TEntity, bool>> EqualExpr<TEntity>(string propertyName)
    {
        if (Equal is null)
            throw new ArgumentNullException(nameof(Equal),
                $"Method {nameof(EqualExpr)} should be invoked only if {nameof(Equal)} filter was filled.");

        var entityProperty = typeof(TEntity).GetProperty(propertyName);
        if (entityProperty is null)
            throw new ArgumentNullException(nameof(entityProperty),
                $"Unable to find property with name {propertyName} in object {typeof(TEntity).Name}.");

        var parameter = Expression.Parameter(typeof(TEntity), "entity");
        return Expression.Lambda<Func<TEntity, bool>>(
            Expression.Equal(
                Expression.Property(
                    parameter,
                    entityProperty),
                Expression.Constant(Equal)),
            parameter);
    }

    object? IGenericPropertyFilter.NotEqual => NotEqual;

    public Expression<Func<TEntity, bool>> NotEqualExpr<TEntity>(string propertyName)
    {
        if (NotEqual is null)
            throw new ArgumentNullException(nameof(NotEqual),
                $"Method {nameof(NotEqualExpr)} should be invoked only if {nameof(NotEqual)} filter was filled.");

        var entityProperty = typeof(TEntity).GetProperty(propertyName);
        if (entityProperty is null)
            throw new ArgumentNullException(nameof(entityProperty),
                $"Unable to find property with name {propertyName} in object {typeof(TEntity).Name}.");

        var parameter = Expression.Parameter(typeof(TEntity), "entity");
        return Expression.Lambda<Func<TEntity, bool>>(
            Expression.NotEqual(
                Expression.Property(
                    parameter,
                    entityProperty),
                Expression.Constant(NotEqual)),
            parameter);
    }

    public Expression<Func<TEntity, bool>> IsNullExpr<TEntity>(string propertyName)
    {
        if (!IsNull.HasValue)
            throw new ArgumentNullException(nameof(IsNull),
                $"Method {nameof(IsNullExpr)} should be invoked only if {nameof(IsNull)} filter was filled.");

        var entityProperty = typeof(TEntity).GetProperty(propertyName);
        if (entityProperty is null)
            throw new ArgumentNullException(nameof(entityProperty),
                $"Unable to find property with name {propertyName} in object {typeof(TEntity).Name}.");

        var parameter = Expression.Parameter(typeof(TEntity), "entity");
        return Expression.Lambda<Func<TEntity, bool>>(
            IsNull.Value
                ? Expression.Equal(
                    Expression.Property(
                        parameter,
                        entityProperty),
                    Expression.Constant(null))
                : Expression.NotEqual(
                    Expression.Property(
                        parameter,
                        entityProperty),
                    Expression.Constant(null)),
            parameter);
    }

    object? IGenericPropertyFilter.GreaterThan => GreaterThan;

    public Expression<Func<TEntity, bool>> GreaterThanExpr<TEntity>(string propertyName)
    {
        if (GreaterThan is null)
            throw new ArgumentNullException(nameof(GreaterThan),
                $"Method {nameof(GreaterThanExpr)} should be invoked only if {nameof(GreaterThan)} filter was filled.");

        var entityProperty = typeof(TEntity).GetProperty(propertyName);
        if (entityProperty is null)
            throw new ArgumentNullException(nameof(entityProperty),
                $"Unable to find property with name {propertyName} in object {typeof(TEntity).Name}.");

        var parameter = Expression.Parameter(typeof(TEntity), "entity");
        return Expression.Lambda<Func<TEntity, bool>>(
            Expression.GreaterThan(
                Expression.Property(
                    parameter,
                    entityProperty),
                Expression.Constant(GreaterThan)),
            parameter);
    }

    object? IGenericPropertyFilter.GreaterThanOrEqual => GreaterThanOrEqual;

    public Expression<Func<TEntity, bool>> GreaterThanOrEqualExpr<TEntity>(string propertyName)
    {
        if (GreaterThanOrEqual is null)
            throw new ArgumentNullException(nameof(GreaterThanOrEqual),
                $"Method {nameof(GreaterThanOrEqualExpr)} should be invoked only if {nameof(GreaterThanOrEqual)} filter was filled.");

        var entityProperty = typeof(TEntity).GetProperty(propertyName);
        if (entityProperty is null)
            throw new ArgumentNullException(nameof(entityProperty),
                $"Unable to find property with name {propertyName} in object {typeof(TEntity).Name}.");

        var parameter = Expression.Parameter(typeof(TEntity), "entity");
        return Expression.Lambda<Func<TEntity, bool>>(
            Expression.GreaterThanOrEqual(
                Expression.Property(
                    parameter,
                    entityProperty),
                Expression.Constant(GreaterThanOrEqual)),
            parameter);
    }

    object? IGenericPropertyFilter.LessThan => LessThan;

    public Expression<Func<TEntity, bool>> LessThanExpr<TEntity>(string propertyName)
    {
        if (LessThan is null)
            throw new ArgumentNullException(nameof(LessThan),
                $"Method {nameof(LessThanExpr)} should be invoked only if {nameof(LessThan)} filter was filled.");

        var entityProperty = typeof(TEntity).GetProperty(propertyName);
        if (entityProperty is null)
            throw new ArgumentNullException(nameof(entityProperty),
                $"Unable to find property with name {propertyName} in object {typeof(TEntity).Name}.");

        var parameter = Expression.Parameter(typeof(TEntity), "entity");
        return Expression.Lambda<Func<TEntity, bool>>(
            Expression.LessThan(
                Expression.Property(
                    parameter,
                    entityProperty),
                Expression.Constant(LessThan)),
            parameter);
    }

    object? IGenericPropertyFilter.LessThanOrEqual => LessThanOrEqual;

    public Expression<Func<TEntity, bool>> LessThanOrEqualExpr<TEntity>(string propertyName)
    {
        if (LessThanOrEqual is null)
            throw new ArgumentNullException(nameof(LessThanOrEqual),
                $"Method {nameof(LessThanOrEqualExpr)} should be invoked only if {nameof(LessThanOrEqual)} filter was filled.");

        var entityProperty = typeof(TEntity).GetProperty(propertyName);
        if (entityProperty is null)
            throw new ArgumentNullException(nameof(entityProperty),
                $"Unable to find property with name {propertyName} in object {typeof(TEntity).Name}.");

        var parameter = Expression.Parameter(typeof(TEntity), "entity");
        return Expression.Lambda<Func<TEntity, bool>>(
            Expression.LessThanOrEqual(
                Expression.Property(
                    parameter,
                    entityProperty),
                Expression.Constant(LessThanOrEqual)),
            parameter);
    }

    IEnumerable? IGenericPropertyFilter.In => In;

    public Expression<Func<TEntity, bool>> InExpr<TEntity>(string propertyName)
    {
        if (In is null)
            throw new ArgumentNullException(nameof(In),
                $"Method {nameof(InExpr)} should be invoked only if {nameof(In)} filter was filled.");

        var entityProperty = typeof(TEntity).GetProperty(propertyName);
        if (entityProperty is null)
            throw new ArgumentNullException(nameof(entityProperty),
                $"Unable to find property with name {propertyName} in object {typeof(TEntity).Name}.");

        var parameter = Expression.Parameter(typeof(TEntity), "entity");
        var member = Expression.Property(parameter, propertyName);
        var method = (
                typeof(Enumerable)
                    .GetMethods(BindingFlags.Static | BindingFlags.Public)
                    .Where(m => m.Name == nameof(Enumerable.Contains))
                    .FirstOrDefault(m => m.GetParameters().Length == 2)
                ?? throw new AutoBackendException(
                    $"Unable to find method {nameof(Enumerable.Contains)} with two parameters."))
            .MakeGenericMethod(member.Type);
        return Expression.Lambda<Func<TEntity, bool>>(
            Expression.Call(
                method,
                new Expression[]
                {
                    Expression.Constant(In),
                    member
                }),
            parameter);
    }
}