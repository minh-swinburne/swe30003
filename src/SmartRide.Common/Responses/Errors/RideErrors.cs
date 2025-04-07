using SmartRide.Common.Constants;

namespace SmartRide.Common.Responses.Errors;

public class RideErrors
{
    public static readonly string Module = "Ride";

    public static readonly ResponseInfo ID_EMPTY = new()
    {
        Code = $"{Module}.ID_EMPTY",
        Message = "Ride ID cannot be empty."
    };

    public static readonly ResponseInfo ID_INVALID = new()
    {
        Code = $"{Module}.ID_INVALID",
        Message = "Ride ID must be a valid GUID."
    };

    public static readonly ResponseInfo ID_NOT_FOUND = new()
    {
        Code = $"{Module}.ID_NOT_FOUND",
        Message = "Ride with ID {RideId} not found."
    };

    public static readonly ResponseInfo PASSENGER_ID_EMPTY = new()
    {
        Code = $"{Module}.PASSENGER_ID_EMPTY",
        Message = "Passenger ID cannot be empty."
    };

    public static readonly ResponseInfo PASSENGER_ID_INVALID = new()
    {
        Code = $"{Module}.PASSENGER_ID_INVALID",
        Message = "Passenger ID must be a valid GUID."
    };


    public static readonly ResponseInfo PASSENGER_ID_NOT_FOUND = new()
    {
        Code = $"{Module}.PASSENGER_ID_NOT_FOUND",
        Message = "Passenger with ID {UserId} not found."
    };

    public static readonly ResponseInfo DRIVER_ID_EMPTY = new()
    {
        Code = $"{Module}.DRIVER_ID_EMPTY",
        Message = "Driver ID cannot be empty."
    };

    public static readonly ResponseInfo DRIVER_ID_INVALID = new()
    {
        Code = $"{Module}.DRIVER_ID_INVALID",
        Message = "Driver ID must be a valid GUID."
    };

    public static readonly ResponseInfo DRIVER_ID_NOT_FOUND = new()
    {
        Code = $"{Module}.DRIVER_ID_NOT_FOUND",
        Message = "Driver with ID {UserId} not found."
    };

    public static readonly ResponseInfo VEHICLE_ID_EMPTY = new()
    {
        Code = $"{Module}.VEHICLE_ID_EMPTY",
        Message = "Vehicle ID cannot be empty."
    };

    public static readonly ResponseInfo VEHICLE_ID_INVALID = new()
    {
        Code = $"{Module}.VEHICLE_ID_INVALID",
        Message = "Vehicle ID must be a valid GUID."
    };

    public static readonly ResponseInfo VEHICLE_ID_NOT_FOUND = new()
    {
        Code = $"{Module}.VEHICLE_ID_NOT_FOUND",
        Message = "Vehicle with ID {VehicleId} not found."
    };

    public static readonly ResponseInfo FARE_INVALID = new()
    {
        Code = $"{Module}.FARE_INVALID",
        Message = $"Fare must be between {RideConstants.MinFare} and {RideConstants.MaxFare}."
    };

    public static readonly ResponseInfo NOTES_TOO_LONG = new()
    {
        Code = $"{Module}.NOTES_TOO_LONG",
        Message = $"Notes cannot exceed {RideConstants.NotesMaxLength} characters."
    };

    public static readonly ResponseInfo PICKUP_LOCATION_ID_EMPTY = new()
    {
        Code = $"{Module}.PICKUP_LOCATION_ID_EMPTY",
        Message = "Pickup location ID cannot be empty."
    };

    public static readonly ResponseInfo DESTINATION_ID_EMPTY = new()
    {
        Code = $"{Module}.DESTINATION_ID_EMPTY",
        Message = "Destination ID cannot be empty."
    };

    public static readonly ResponseInfo DRIVER_ROLE_INVALID = new()
    {
        Code = $"{Module}.DRIVER_ROLE_INVALID",
        Message = "The specified DriverId does not reference a User with the Driver role."
    };

    public static readonly ResponseInfo PASSENGER_ROLE_INVALID = new()
    {
        Code = $"{Module}.PASSENGER_ROLE_INVALID",
        Message = "The specified PassengerId does not reference a User with the Passenger role."
    };

    public static readonly ResponseInfo VEHICLE_OWNERSHIP_INVALID = new()
    {
        Code = $"{Module}.VEHICLE_OWNERSHIP_INVALID",
        Message = "The specified VehicleId does not reference a Vehicle owned by the specified DriverId."
    };

    public static readonly ResponseInfo PICKUP_ETA_IN_PAST = new()
    {
        Code = $"{Module}.PICKUP_ETA_IN_PAST",
        Message = "Pickup ETA must not be in the past."
    };

    public static readonly ResponseInfo ARRIVAL_ETA_IN_PAST = new()
    {
        Code = $"{Module}.ARRIVAL_ETA_IN_PAST",
        Message = "Arrival ETA must not be in the past."
    };

    public static readonly ResponseInfo PICKUP_ATA_IN_FUTURE = new()
    {
        Code = $"{Module}.PICKUP_ATA_IN_FUTURE",
        Message = "Pickup ATA must not be in the future."
    };

    public static readonly ResponseInfo ARRIVAL_ATA_IN_FUTURE = new()
    {
        Code = $"{Module}.ARRIVAL_ATA_IN_FUTURE",
        Message = "Arrival ATA must not be in the future."
    };
}
