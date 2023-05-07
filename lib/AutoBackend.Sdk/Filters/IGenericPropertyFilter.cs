using System.Collections;
using System.Linq.Expressions;

namespace AutoBackend.Sdk.Filters;

internal interface IGenericPropertyFilter
{
    object? Equal { get; }
    object? NotEqual { get; }
    bool? IsNull { get; }
    object? GreaterThan { get; }
    object? GreaterThanOrEqual { get; }
    object? LessThan { get; }
    object? LessThanOrEqual { get; }
    IEnumerable? In { get; }
    Expression<Func<TEntity, bool>> EqualExpr<TEntity>(string propertyName);
    Expression<Func<TEntity, bool>> NotEqualExpr<TEntity>(string propertyName);
    Expression<Func<TEntity, bool>> IsNullExpr<TEntity>(string propertyName);
    Expression<Func<TEntity, bool>> GreaterThanExpr<TEntity>(string propertyName);
    Expression<Func<TEntity, bool>> GreaterThanOrEqualExpr<TEntity>(string propertyName);
    Expression<Func<TEntity, bool>> LessThanExpr<TEntity>(string propertyName);
    Expression<Func<TEntity, bool>> LessThanOrEqualExpr<TEntity>(string propertyName);
    Expression<Func<TEntity, bool>> InExpr<TEntity>(string propertyName);
}