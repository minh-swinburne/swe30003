namespace SmartRide.Common.Responses.Errors;

public static class VehicleErrors
{
    public static readonly string Module = "Vehicle";

    public static readonly ResponseInfo ID_EMPTY = new()
    {
        Code = $"{Module}.ID_EMPTY",
        Message = "Vehicle ID cannot be empty."
    };

    public static readonly ResponseInfo ID_INVALID = new()
    {
        Code = $"{Module}.ID_INVALID",
        Message = "Vehicle ID must be a valid GUID."
    };

    public static readonly ResponseInfo ID_NOT_FOUND = new()
    {
        Code = $"{Module}.ID_NOT_FOUND",
        Message = "Vehicle with ID {VehicleId} not found."
    };

    public static readonly ResponseInfo VIN_EMPTY = new()
    {
        Code = $"{Module}.VIN_EMPTY",
        Message = "VIN cannot be empty."
    };

    public static readonly ResponseInfo VIN_TOO_LONG = new()
    {
        Code = $"{Module}.VIN_TOO_LONG",
        Message = "VIN cannot be longer than 17 characters."
    };

    public static readonly ResponseInfo VIN_INVALID = new()
    {
        Code = $"{Module}.VIN_INVALID",
        Message = "VIN must match the required format."
    };

    public static readonly ResponseInfo VIN_NOT_FOUND = new()
    {
        Code = $"{Module}.VIN_NOT_FOUND",
        Message = "Vehicle with VIN {Vin} not found."
    };

    public static readonly ResponseInfo PLATE_EMPTY = new()
    {
        Code = $"{Module}.PLATE_EMPTY",
        Message = "Plate cannot be empty."
    };

    public static readonly ResponseInfo PLATE_TOO_LONG = new()
    {
        Code = $"{Module}.PLATE_TOO_LONG",
        Message = "Plate cannot be longer than 10 characters."
    };

    public static readonly ResponseInfo PLATE_INVALID = new()
    {
        Code = $"{Module}.PLATE_INVALID",
        Message = "Plate must match the required format."
    };

    public static readonly ResponseInfo PLATE_NOT_FOUND = new()
    {
        Code = $"{Module}.PLATE_NOT_FOUND",
        Message = "Vehicle with plate {Plate} not found."
    };

    public static readonly ResponseInfo MAKE_EMPTY = new()
    {
        Code = $"{Module}.MAKE_EMPTY",
        Message = "Make cannot be empty."
    };

    public static readonly ResponseInfo MAKE_TOO_LONG = new()
    {
        Code = $"{Module}.MAKE_TOO_LONG",
        Message = "Make cannot be longer than 50 characters."
    };

    public static readonly ResponseInfo MODEL_EMPTY = new()
    {
        Code = $"{Module}.MODEL_EMPTY",
        Message = "Model cannot be empty."
    };

    public static readonly ResponseInfo MODEL_TOO_LONG = new()
    {
        Code = $"{Module}.MODEL_TOO_LONG",
        Message = "Model cannot be longer than 50 characters."
    };

    public static readonly ResponseInfo YEAR_INVALID = new()
    {
        Code = $"{Module}.YEAR_INVALID",
        Message = "Year must be a valid integer."
    };

    public static readonly ResponseInfo REGISTERED_DATE_INVALID = new()
    {
        Code = $"{Module}.REGISTERED_DATE_INVALID",
        Message = "Registered date must be a valid date."
    };
}
