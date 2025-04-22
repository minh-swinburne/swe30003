using SmartRide.Common.Constants;

namespace SmartRide.Common.Responses.Errors;

public static class LocationErrors
{
    public static readonly string Module = "Location";

    public static readonly ResponseInfo ID_EMPTY = new()
    {
        Code = $"{Module}.ID_EMPTY",
        Message = "Location ID cannot be empty."
    };

    public static readonly ResponseInfo ID_INVALID = new()
    {
        Code = $"{Module}.ID_INVALID",
        Message = "Location ID must be a valid GUID."
    };

    public static readonly ResponseInfo ID_NOT_FOUND = new()
    {
        Code = $"{Module}.ID_NOT_FOUND",
        Message = "Location with ID {LocationId} not found."
    };

    public static readonly ResponseInfo ADDRESS_EMPTY = new()
    {
        Code = $"{Module}.ADDRESS_EMPTY",
        Message = "Address cannot be empty."
    };

    public static readonly ResponseInfo ADDRESS_TOO_LONG = new()
    {
        Code = $"{Module}.ADDRESS_TOO_LONG",
        Message = $"Address cannot exceed {LocationConstants.AddressMaxLength} characters."
    };

    public static readonly ResponseInfo LATITUDE_INVALID = new()
    {
        Code = $"{Module}.LATITUDE_INVALID",
        Message = "Latitude must be a valid number."
    };

    public static readonly ResponseInfo LONGITUDE_INVALID = new()
    {
        Code = $"{Module}.LONGITUDE_INVALID",
        Message = "Longitude must be a valid number."
    };

    public static readonly ResponseInfo USER_ID_INVALID = new()
    {
        Code = $"{Module}.USER_ID_INVALID",
        Message = "User ID must be a valid GUID."
    };

    public static readonly ResponseInfo USER_ID_NOT_FOUND = new()
    {
        Code = $"{Module}.USER_ID_NOT_FOUND",
        Message = "User with ID {UserId} not found."
    };

    public static readonly ResponseInfo CREATE_REQUEST_INVALID = new()
    {
        Code = $"{Module}.CREATE_REQUEST_INVALID",
        Message = "Either address or coordinates must be provided."
    };
}
