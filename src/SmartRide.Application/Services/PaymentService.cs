using MediatR;
using SmartRide.Application.Commands;
using SmartRide.Application.Commands.Payments;
using SmartRide.Application.DTOs;
using SmartRide.Application.DTOs.Payments;
using SmartRide.Application.Factories;
using SmartRide.Application.Interfaces;
using SmartRide.Application.Queries.Payments;

namespace SmartRide.Application.Services;

public class PaymentService(IMediator mediator) : IPaymentService
{
    private readonly IMediator _mediator = mediator;

    public async Task<ListResponseDTO<ListPaymentResponseDTO>> ListPaymentsAsync(ListPaymentRequestDTO request)
    {
        var query = MediatRFactory.CreateQuery<ListPaymentQuery>(request);
        var result = await _mediator.Send(query);
        return new ListResponseDTO<ListPaymentResponseDTO>
        {
            Data = result,
            Count = result.Count
        };
    }

    public async Task<ResponseDTO<GetPaymentResponseDTO>> GetPaymentByIdAsync(GetPaymentByIdRequestDTO request)
    {
        var query = MediatRFactory.CreateQuery<GetPaymentByIdQuery>(request);
        var result = await _mediator.Send(query);
        return new ResponseDTO<GetPaymentResponseDTO> { Data = result };
    }

    public async Task<ResponseDTO<UpdatePaymentResponseDTO>> UpdatePaymentAsync(UpdatePaymentRequestDTO request)
    {
        var command = MediatRFactory.CreateCommand<UpdatePaymentCommand>(request);
        var result = await _mediator.Send(command);
        await _mediator.Send(new SaveChangesCommand());
        return new ResponseDTO<UpdatePaymentResponseDTO> { Data = result };
    }
}
