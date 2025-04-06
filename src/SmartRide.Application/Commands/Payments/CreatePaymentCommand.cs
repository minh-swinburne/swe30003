using SmartRide.Application.DTOs.Payments;
using SmartRide.Domain.Enums;

namespace SmartRide.Application.Commands.Payments;

public class CreatePaymentCommand : BaseCommand<CreatePaymentResponseDTO>
{
    public Guid RideId { get; set; }
    public decimal Amount { get; set; }
    public PaymentMethodEnum PaymentMethodId { get; set; }
}
