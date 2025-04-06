namespace SmartRide.Application.DTOs.Payments;

public class DeletePaymentRequestDTO : BaseRequestDTO
{
    public required Guid PaymentId { get; set; }
}
