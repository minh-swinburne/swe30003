using SmartRide.Application.DTOs.Payments;

namespace SmartRide.Application.Queries.Payments;

public class GetPaymentByRideIdQuery : BaseQuery<GetPaymentResponseDTO>
{
    public Guid RideId { get; set; }
}
