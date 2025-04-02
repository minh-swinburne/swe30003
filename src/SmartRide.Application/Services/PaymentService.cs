using MediatR;
using SmartRide.Application.Commands;
using SmartRide.Application.DTOs;
using SmartRide.Application.DTOs.Payments;
using SmartRide.Application.Factories;
using SmartRide.Application.Interfaces;

namespace SmartRide.Application.Services
{
    public class PaymentService(IMediator mediator) : IPaymentService
    {
        private readonly IMediator _mediator = mediator;

        public async Task<ResponseDTO<ExecutePaymentResponseDTO>> ExecutePaymentAsync(ExecutePaymentRequestDTO request)
        {
            // Map PaymentRequestDTO to PaymentCommand
            var command = MediatRFactory.CreateCommand<ExecutePaymentCommand>(request);

            return await _mediator.Send(command);
        }
    }
}
