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
    /// <returns>A new predicament representing the logical AND or OR of the two input predicates.</returns>
    public static Expression<Func<T, bool>> AddFilter<T>(this Expression<Func<T, bool>>? left, Expression<Func<T, bool>> right, bool useOr = false)
    {
        if (left == null) return right;

        var parameter = Expression.Parameter(typeof(T));

        var leftVisitor = new ReplaceExpressionVisitor(left.Parameters[0], parameter);
        var leftBody = leftVisitor.Visit(left.Body);

        var rightVisitor = new ReplaceExpressionVisitor(right.Parameters[0], parameter);
        var rightBody = rightVisitor.Visit(right.Body);

        Expression combinedBody;

        if (useOr)
            combinedBody = Expression.OrElse(leftBody, rightBody);
        else
            combinedBody = Expression.AndAlso(leftBody, rightBody);

        return Expression.Lambda<Func<T, bool>>(combinedBody, parameter);
    }

    // Helper class to replace expression parameters
    private class ReplaceExpressionVisitor(Expression oldValue, Expression newValue) : ExpressionVisitor
    {
        private readonly Expression _newValue = newValue;
        private readonly Expression _oldValue = oldValue;
    }
}
