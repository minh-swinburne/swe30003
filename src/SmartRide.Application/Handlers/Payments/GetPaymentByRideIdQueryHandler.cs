using AutoMapper;
using SmartRide.Application.DTOs.Payments;
using SmartRide.Application.Queries.Payments;
using SmartRide.Common.Exceptions;
using SmartRide.Common.Responses.Errors;
using SmartRide.Domain.Entities.Base;
using SmartRide.Domain.Interfaces;

namespace SmartRide.Application.Handlers.Payments;

public class GetPaymentByRideIdQueryHandler(IRepository<Payment> paymentRepository, IMapper mapper)
    : BaseQueryHandler<GetPaymentByRideIdQuery, GetPaymentResponseDTO>
{
    private readonly IRepository<Payment> _paymentRepository = paymentRepository;
    private readonly IMapper _mapper = mapper;

    public override async Task<GetPaymentResponseDTO> Handle(GetPaymentByRideIdQuery query, CancellationToken cancellationToken)
    {
        var payment = await _paymentRepository
            .Query(filter: p => p.RideId == query.RideId, includes: [
                p => p.Ride,
                p => p.PaymentMethod
            ], cancellationToken)
            ?? throw new BaseException(PaymentErrors.Module, PaymentErrors.RIDE_ID_NOT_FOUND.FormatMessage(("RideId", query.RideId)));

        return _mapper.Map<GetPaymentResponseDTO>(payment);
    }
}
