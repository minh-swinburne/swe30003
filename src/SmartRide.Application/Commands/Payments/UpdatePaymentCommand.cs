using SmartRide.Application.DTOs.Payments;
using SmartRide.Domain.Enums;

namespace SmartRide.Application.Commands.Payments;

public class UpdatePaymentCommand : BaseCommand<UpdatePaymentResponseDTO>
{
    public Guid PaymentId { get; set; }
    public decimal? Amount { get; set; }
    public string? Currency { get; set; }
    public PaymentMethodEnum? PaymentMethodId { get; set; }
    public PaymentStatusEnum? Status { get; set; }
    public DateTime? TransactionTime { get; set; }
    public string? TransactionId { get; set; }
}
