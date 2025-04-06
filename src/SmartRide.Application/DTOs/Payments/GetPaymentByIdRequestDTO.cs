namespace SmartRide.Application.DTOs.Payments;

public class GetPaymentByIdRequestDTO : BaseRequestDTO
{
    public required Guid PaymentId { get; init; }
}
