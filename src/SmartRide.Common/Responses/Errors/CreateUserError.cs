namespace SmartRide.Common.Responses.Errors;

public class CreateUserError
{
    public static readonly string Module = "User";
    public static readonly ResponseInfo ERROR_001A = new()
    {
        Code = $"{Module}.ERROR_001A",
        Message = "Firstname can not be empty."
    };
    public static readonly ResponseInfo ERROR_001B = new()
    {
        Code = $"{Module}.ERROR_001B",
        Message = "Firstname can not be longer than 50 characters."
    };
    public static readonly ResponseInfo ERROR_001C = new()
    {
        Code = $"{Module}.ERROR_001C",
        Message = "Firstname can only contain alphanumeric characters or whitespaces."
    };
    public static readonly ResponseInfo ERROR_002A = new()
    {
        Code = $"{Module}.ERROR_002A",
        Message = "Lastname can not be longer than 50 characters."
    };
    public static readonly ResponseInfo ERROR_002B = new()
    {
        Code = $"{Module}.ERROR_002B",
        Message = "Lastname can only contain alphanumeric characters or whitespaces."
    };
    public static readonly ResponseInfo ERROR_003A = new()
    {
        Code = $"{Module}.ERROR_003A",
        Message = "Email can not be empty."
    };
    public static readonly ResponseInfo ERROR_003B = new()
    {
        Code = $"{Module}.ERROR_003B",
        Message = "Email can not be longer than 255 characters."
    };
    public static readonly ResponseInfo ERROR_003C = new()
    {
        Code = $"{Module}.ERROR_003C",
        Message = "Email must be a valid email address."
    };
    public static readonly ResponseInfo ERROR_004A = new()
    {
        Code = $"{Module}.ERROR_004A",
        Message = "Phone number can not be empty."
    };
    public static readonly ResponseInfo ERROR_004B = new()
    {
        Code = $"{Module}.ERROR_004B",
        Message = "Phone number can not be longer than 45 characters."
    };
    public static readonly ResponseInfo ERROR_004C = new()
    {
        Code = $"{Module}.ERROR_004C",
        Message = "Phone must be a valid E.164 phone number."
    };
    public static readonly ResponseInfo ERROR_005A = new()
    {
        Code = $"{Module}.ERROR_005A",
        Message = "Password can not be empty."
    };
    public static readonly ResponseInfo ERROR_005B = new()
    {
        Code = $"{Module}.ERROR_005B",
        Message = "Password must be at least 8 characters long."
    };
    public static readonly ResponseInfo ERROR_005C = new()
    {
        Code = $"{Module}.ERROR_005C",
        Message = "Password can not be longer than 150 characters."
    };
    public static readonly ResponseInfo ERROR_005D = new()
    {
        Code = $"{Module}.ERROR_005D",
        Message = "Password must include at least one letter, one number, and one special character."
    };
}
