using System.Linq.Expressions;
using System.Reflection;

namespace SmartRide.Common.Helpers;

public static class QueryHelper
{
    public static PropertyInfo[] GetProperties<T>()
    {
        return typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
    }

    public static PropertyInfo? GetProperty<T>(string propertyName, bool ignoreCase = true)
    {
        BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Instance;
        if (ignoreCase)
        {
            bindingFlags |= BindingFlags.IgnoreCase;
        }

        return typeof(T).GetProperty(propertyName, bindingFlags);
    }

    public static Expression<Func<T, object>> GetSortExpression<T>(PropertyInfo propertyInfo)
    {
        var parameter = Expression.Parameter(typeof(T), "x");
        var propertyAccess = Expression.Property(parameter, propertyInfo);
        return Expression.Lambda<Func<T, object>>(Expression.Convert(propertyAccess, typeof(object)), parameter);
    }
}
