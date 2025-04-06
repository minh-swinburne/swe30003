namespace SmartRide.Common.Constants;

public readonly record struct UserConstants
{
    public const string EmailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
    public const string NamePattern = @"^[A-Z][a-zA-Z\s]*$";
    public const string PhonePattern = @"^\+[1-9]\d{0,2}(\s?\d{1,4}){1,3}$";
    public const string PasswordPattern = @"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]+$";

    public const int NameMaxLength = 50;
    public const int EmailMaxLength = 255;
    public const int PhoneMaxLength = 45;
    public const int PasswordMinLength = 8;
    public const int PasswordMaxLength = 150;
}
