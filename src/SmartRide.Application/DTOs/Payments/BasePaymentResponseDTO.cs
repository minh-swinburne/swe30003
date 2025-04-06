namespace SmartRide.Application.DTOs.Payments;

public abstract class BasePaymentResponseDTO
{
    public required Guid PaymentId { get; set; }
}
