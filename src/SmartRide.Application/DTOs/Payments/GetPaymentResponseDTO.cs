using SmartRide.Application.DTOs.Lookup;
using SmartRide.Application.DTOs.Rides;
using SmartRide.Domain.Enums;

namespace SmartRide.Application.DTOs.Payments;

public class GetPaymentResponseDTO : BasePaymentResponseDTO
{
    public required GetRideResponseDTO Ride { get; set; }
    public required decimal Amount { get; set; }
    public required string Currency { get; set; }
    public required PaymentMethodDTO PaymentMethod { get; set; }
    public required PaymentStatusEnum Status { get; set; }
    public DateTime? TransactionTime { get; set; }
    public string? TransactionId { get; set; }
}
