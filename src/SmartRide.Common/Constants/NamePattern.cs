namespace SmartRide.Common.Constants;

public readonly record struct NamePattern
{
    public const string Regex = @"^[A-Za-z\s]+$";
    public const string Description = "Name can only contain alphabetic or whitespace characters.";
}
