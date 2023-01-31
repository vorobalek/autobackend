using System.Collections;
using System.Linq.Expressions;

namespace AutoBackend.Sdk.Filters.Generic;

internal interface IGenericFilter
{
    public object? Equal { get; }
    public object? NotEqual { get; }
    public bool? IsNull { get; }
    public object? GreaterThan { get; }
    public object? GreaterThanOrEqual { get; }
    public object? LessThan { get; }
    public object? LessThanOrEqual { get; }
    public IEnumerable? In { get; }
    public Expression<Func<TEntity, bool>> EqualExpr<TEntity>(string propertyName);
    public Expression<Func<TEntity, bool>> NotEqualExpr<TEntity>(string propertyName);
    public Expression<Func<TEntity, bool>> IsNullExpr<TEntity>(string propertyName);
    public Expression<Func<TEntity, bool>> GreaterThanExpr<TEntity>(string propertyName);
    public Expression<Func<TEntity, bool>> GreaterThanOrEqualExpr<TEntity>(string propertyName);
    public Expression<Func<TEntity, bool>> LessThanExpr<TEntity>(string propertyName);
    public Expression<Func<TEntity, bool>> LessThanOrEqualExpr<TEntity>(string propertyName);
    public Expression<Func<TEntity, bool>> InExpr<TEntity>(string propertyName);
}