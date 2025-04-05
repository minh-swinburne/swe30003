namespace SmartRide.Common.Constants;

public readonly record struct PhonePattern
{
    public const string Regex = @"^\+[1-9]\d{0,2}(\s?\d{1,4}){1,3}$";
    public const string Description = "Phone must be a valid E.164 phone number.";
}
