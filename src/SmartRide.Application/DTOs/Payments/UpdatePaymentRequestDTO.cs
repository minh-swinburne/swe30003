using SmartRide.Domain.Enums;

namespace SmartRide.Application.DTOs.Payments;

public class UpdatePaymentRequestDTO : BaseRequestDTO
{
    public required Guid PaymentId { get; set; }
    public decimal? Amount { get; set; }
    public PaymentMethodEnum? PaymentMethodId { get; set; }
    public PaymentStatusEnum? Status { get; set; }
    public DateTime? TransactionTime { get; set; }
}
