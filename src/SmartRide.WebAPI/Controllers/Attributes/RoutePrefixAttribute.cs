namespace SmartRide.WebAPI.Controllers.Attributes;

[AttributeUsage(AttributeTargets.Class)] // Apply only to classes (controllers)
public class RoutePrefixAttribute(string prefix) : Attribute
{
    public string Prefix { get; } = prefix;
}
