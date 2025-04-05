namespace SmartRide.Common.Constants;

public readonly record struct EmailPattern
{
    public const string Regex = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
    public const string Description = "Email must be a valid email address.";
}
