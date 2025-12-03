using System.Linq.Expressions;

namespace AutoBackend.Sdk.Builders;

internal static class ExpressionBuilder
{
    internal static Expression<Func<T, bool>> True<T>()
    {
        return _ => true;
    }

    internal static Expression<Func<T, bool>> False<T>()
    {
        return _ => false;
    }

    extension<T>(Expression<Func<T, bool>> expr1)
    {
        internal Expression<Func<T, bool>> Or(Expression<Func<T, bool>> expr2)
        {
            var invokedExpr = Expression.Invoke(expr2, expr1.Parameters);
            return Expression.Lambda<Func<T, bool>>(
                Expression.OrElse(
                    expr1.Body,
                    invokedExpr),
                expr1.Parameters);
        }

        internal Expression<Func<T, bool>> And(Expression<Func<T, bool>> expr2)
        {
            var invokedExpr = Expression.Invoke(expr2, expr1.Parameters);
            return Expression.Lambda<Func<T, bool>>(
                Expression.AndAlso(
                    expr1.Body,
                    invokedExpr),
                expr1.Parameters);
        }
    }
}