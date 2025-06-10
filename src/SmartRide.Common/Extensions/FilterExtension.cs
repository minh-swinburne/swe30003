using System.Linq.Expressions;

namespace SmartRide.Common.Extensions;

public static class FilterExtension
{
    /// <summary>
    /// Add a filter to an existing expression
    /// </summary>
    /// <typeparam name="T">The type of the predicate parameter.</typeparam>
    /// <param name="left">The left predicate. If null, the right predicate is returned.</param>
    /// <param name="right">The right predicate.</param>
    /// <param name="useOr">Whether to use AND (default) or OR operation.</param>
    /// <returns>A new predicate representing the logical AND or OR of the two input predicates.</returns>
    public static Expression<Func<T, bool>> AddFilter<T>(this Expression<Func<T, bool>>? left, Expression<Func<T, bool>> right, bool useOr = false)
    {
        if (left == null) return right;

        var parameter = Expression.Parameter(typeof(T));

        var leftBody = ReplaceParameter(left.Body, left.Parameters[0], parameter);
        var rightBody = ReplaceParameter(right.Body, right.Parameters[0], parameter);

        Expression combinedBody = useOr
            ? Expression.OrElse(leftBody, rightBody)
            : Expression.AndAlso(leftBody, rightBody);

        return Expression.Lambda<Func<T, bool>>(combinedBody, parameter);
    }

    /// <summary>
    /// Replaces the parameter in an expression with a new parameter.
    /// </summary>
    private static Expression ReplaceParameter(Expression body, ParameterExpression oldParameter, ParameterExpression newParameter)
    {
        return new ReplaceExpressionVisitor(oldParameter, newParameter).Visit(body)!;
    }

    // Helper class to replace expression parameters
    private class ReplaceExpressionVisitor(ParameterExpression oldParameter, ParameterExpression newParameter) : ExpressionVisitor
    {
        private readonly ParameterExpression _oldParameter = oldParameter;
        private readonly ParameterExpression _newParameter = newParameter;

        protected override Expression VisitParameter(ParameterExpression node)
        {
            return node == _oldParameter ? _newParameter : base.VisitParameter(node);
        }
    }
}
