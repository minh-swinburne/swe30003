using SmartRide.Application.DTOs.Payments;

namespace SmartRide.Application.Queries.Payments;

public class GetPaymentByIdQuery : BaseQuery<GetPaymentResponseDTO>
{
    public Guid PaymentId { get; set; }
}
