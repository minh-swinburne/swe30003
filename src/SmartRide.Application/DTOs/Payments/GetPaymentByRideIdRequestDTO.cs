namespace SmartRide.Application.DTOs.Payments;

public class GetPaymentByRideIdRequestDTO : BaseRequestDTO
{
    public required Guid RideId { get; init; }
}
