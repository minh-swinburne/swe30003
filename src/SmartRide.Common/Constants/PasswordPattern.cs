namespace SmartRide.Common.Constants;

public readonly record struct PasswordPattern
{
    public const string Regex = @"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]+$";
    public const string Description = "Password must be at least 8 characters long, include at least one letter, one number, and one special character.";
}
