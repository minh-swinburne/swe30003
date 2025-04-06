using SmartRide.Domain.Enums;

namespace SmartRide.Application.DTOs.Payments;

public class CreatePaymentRequestDTO : BaseRequestDTO
{
    public required Guid RideId { get; set; }
    public required decimal Amount { get; set; }
    public required PaymentMethodEnum PaymentMethodId { get; set; }
}
