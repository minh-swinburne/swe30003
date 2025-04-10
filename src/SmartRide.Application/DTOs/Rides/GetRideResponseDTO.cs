using SmartRide.Application.DTOs.Locations;
using SmartRide.Application.DTOs.Lookup;
using SmartRide.Application.DTOs.Payments;
using SmartRide.Application.DTOs.Users;
using SmartRide.Application.DTOs.Vehicles;
using SmartRide.Domain.Enums;

namespace SmartRide.Application.DTOs.Rides;

public class GetRideResponseDTO : BaseRideResponseDTO
{
    public required GetUserResponseDTO Passenger { get; set; }
    public required GetPaymentResponseDTO? Payment { get; set; }
    public GetUserResponseDTO? Driver { get; set; }
    public GetVehicleResponseDTO? Vehicle { get; set; }
    public required VehicleTypeDTO VehicleType { get; set; }
    public required RideTypeEnum RideType { get; set; }
    public required RideStatusEnum RideStatus { get; set; }
    public required GetLocationResponseDTO PickupLocation { get; set; }
    public required GetLocationResponseDTO Destination { get; set; }
    public DateTime? PickupETA { get; set; }
    public DateTime? PickupATA { get; set; }
    public DateTime? ArrivalETA { get; set; }
    public DateTime? ArrivalATA { get; set; }
    public required decimal Fare { get; set; }
    public string? Notes { get; set; }
}
