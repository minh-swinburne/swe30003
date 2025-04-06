namespace SmartRide.Application.DTOs.Payments;

public class DeletePaymentResponseDTO : BasePaymentResponseDTO
{
    public required bool Success { get; set; }
}
