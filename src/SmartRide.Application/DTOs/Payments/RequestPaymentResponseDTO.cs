namespace SmartRide.Application.DTOs.Payments;

public class RequestPaymentResponseDTO : BasePaymentResponseDTO
{
    public required string TransactionId { get; set; }
    public required string ApprovalUrl { get; set; }
}
