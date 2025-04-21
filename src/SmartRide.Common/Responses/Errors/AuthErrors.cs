namespace SmartRide.Common.Responses.Errors;

public class AuthErrors
{
    public static readonly string Module = "Authentication";

    public static readonly ResponseInfo TOKEN_EMPTY = new()
    {
        Code = $"{Module}.TOKEN_EMPTY",
        Message = "Token can not be empty."
    };

    public static readonly ResponseInfo TOKEN_INVALID = new()
    {
        Code = $"{Module}.TOKEN_INVALID",
        Message = "Invalid token."
    };

    public static readonly ResponseInfo TOKEN_EXPIRED = new()
    {
        Code = $"{Module}.TOKEN_EXPIRED",
        Message = "Token has expired."
    };

    public static readonly ResponseInfo USER_NOT_FOUND = new()
    {
        Code = $"{Module}.USER_NOT_FOUND",
        Message = "User not found."
    };

    public static readonly ResponseInfo LOGIN_FAILED = new()
    {
        Code = $"{Module}.LOGIN_FAILED",
        Message = "User doesn't exist or password is incorrect."
    };

    public static readonly ResponseInfo REGISTRATION_FAILED = new()
    {
        Code = $"{Module}.REGISTRATION_FAILED",
        Message = "Registeration failed. Please check your information and try again."
    };

    public static readonly ResponseInfo VALIDATION_FAILED = new()
    {
        Code = $"{Module}.VALIDATION_FAILED",
        Message = "Validation failed. Details: {Details}."
    };
}
