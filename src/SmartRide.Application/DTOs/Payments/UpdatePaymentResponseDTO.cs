using SmartRide.Application.DTOs.Lookup;
using SmartRide.Domain.Enums;

namespace SmartRide.Application.DTOs.Payments;

public class UpdatePaymentResponseDTO : BasePaymentResponseDTO
{
    public required decimal Amount { get; set; }
    public required PaymentMethodDTO PaymentMethod { get; set; }
    public required PaymentStatusEnum Status { get; set; }
    public DateTime? TransactionTime { get; set; }
}
