using System.Reflection;

namespace SmartRide.Common.Extensions;

public static class DictionaryExtension
{
    public static T ToObject<T>(this IDictionary<string, object> source, bool ignoreCase = true)
        where T : class, new()
    {
        var someObject = new T();
        var someObjectType = someObject.GetType();
        var bindingFlags = BindingFlags.Public | BindingFlags.Instance;

        if (source == null || source.Count == 0)
            return someObject;

        if (ignoreCase)
            bindingFlags |= BindingFlags.IgnoreCase;

        foreach (var item in source)
        {
            someObjectType?
                .GetProperty(item.Key, bindingFlags)?
                .SetValue(someObject, item.Value, null);
        }

        return someObject;
    }

    public static Dictionary<string, object?> ToDictionary(
        this object source,
        BindingFlags bindingAttr = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance
    )
    {
        return source.GetType().GetProperties(bindingAttr).ToDictionary
        (
            propInfo => propInfo.Name,
            propInfo => propInfo.GetValue(source, null)
        );
    }
}
