using SmartRide.Application.DTOs.Payments;

namespace SmartRide.Application.Commands.Payments;

public class DeletePaymentCommand : BaseCommand<DeletePaymentResponseDTO>
{
    public Guid PaymentId { get; set; }
}
