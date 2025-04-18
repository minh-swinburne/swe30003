using MediatR;
using SmartRide.Application.Commands;
using SmartRide.Application.Commands.Payments;
using SmartRide.Application.DTOs;
using SmartRide.Application.DTOs.Payments;
using SmartRide.Application.Factories;
using SmartRide.Application.Interfaces;
using SmartRide.Application.Queries.Payments;
using SmartRide.Application.Queries.Rides;
using SmartRide.Common.Responses;
using SmartRide.Common.Responses.Errors;
using SmartRide.Domain.Enums;
using SmartRide.Domain.Interfaces;

namespace SmartRide.Application.Services;

public class PaymentService(IMediator mediator, ITransactionService transactionService) : IPaymentService
{
    private readonly IMediator _mediator = mediator;
    private readonly ITransactionService _transactionService = transactionService;

    public async Task<ListResponseDTO<ListPaymentResponseDTO>> ListPaymentsAsync(ListPaymentRequestDTO request)
    {
        try
        {
            var query = MediatRFactory.CreateQuery<ListPaymentQuery>(request);
            var result = await _mediator.Send(query);
            return new ListResponseDTO<ListPaymentResponseDTO>
            {
                Data = result,
                Count = result.Count
            };
        }
        catch (Exception ex)
        {
            return new ListResponseDTO<ListPaymentResponseDTO>
            {
                Info = new ResponseInfo { Code = "LIST_PAYMENTS_ERROR", Message = ex.Message }
            };
        }
    }

    public async Task<ResponseDTO<GetPaymentResponseDTO>> GetPaymentByIdAsync(GetPaymentByIdRequestDTO request)
    {
        try
        {
            var query = MediatRFactory.CreateQuery<GetPaymentByIdQuery>(request);
            var result = await _mediator.Send(query);
            return new ResponseDTO<GetPaymentResponseDTO> { Data = result };
        }
        catch (Exception ex)
        {
            return new ResponseDTO<GetPaymentResponseDTO>
            {
                Info = new ResponseInfo { Code = "GET_PAYMENT_BY_ID_ERROR", Message = ex.Message }
            };
        }
    }

    public async Task<ResponseDTO<UpdatePaymentResponseDTO>> UpdatePaymentAsync(UpdatePaymentRequestDTO request)
    {
        try
        {
            var command = MediatRFactory.CreateCommand<UpdatePaymentCommand>(request);
            var result = await _mediator.Send(command);
            await _mediator.Send(new SaveChangesCommand());
            return new ResponseDTO<UpdatePaymentResponseDTO> { Data = result };
        }
        catch (Exception ex)
        {
            return new ResponseDTO<UpdatePaymentResponseDTO>
            {
                Info = new ResponseInfo { Code = "UPDATE_PAYMENT_ERROR", Message = ex.Message }
            };
        }
    }

    public async Task<ResponseDTO<RequestPaymentResponseDTO>> RequestPaymentAsync(GetPaymentByIdRequestDTO request)
    {
        GetPaymentResponseDTO payment = null!;

        try
        {
            var paymentQuery = MediatRFactory.CreateQuery<GetPaymentByIdQuery>(request);
            payment = await _mediator.Send(paymentQuery);

            var rideQuery = new GetRideByIdQuery
            {
                RideId = payment.Ride.RideId
            };
            var ride = await _mediator.Send(rideQuery);

            var (transactionId, approvalUrl) = await _transactionService.CreateTransactionAsync(
                payment.PaymentMethod.PaymentMethodId,
                payment.Amount,
                payment.Currency,
                ride.RideType,
                ride.VehicleType.VehicleTypeId,
                ride.PickupLocation.Address,
                ride.Destination.Address
            );

            var command = new UpdatePaymentCommand
            {
                PaymentId = payment.PaymentId,
                TransactionId = transactionId,
                Status = PaymentStatusEnum.Processing
            };

            var result = await _mediator.Send(command);
            await _mediator.Send(new SaveChangesCommand());

            return new ResponseDTO<RequestPaymentResponseDTO>
            {
                Data = new RequestPaymentResponseDTO
                {
                    PaymentId = result.PaymentId,
                    TransactionId = transactionId,
                    ApprovalUrl = approvalUrl,
                }
            };
        }
        catch (Exception ex)
        {
            await HandlePaymentErrorAsync(payment.PaymentId);
            return new ResponseDTO<RequestPaymentResponseDTO>
            {
                Info = new ResponseInfo
                {
                    Code = "REQUEST_PAYMENT_ERROR",
                    Message = ex.Message
                }
            };
        }
    }

    public async Task<ResponseDTO<UpdatePaymentResponseDTO>> CapturePaymentAsync(GetPaymentByIdRequestDTO request)
    {
        try
        {
            var paymentQuery = MediatRFactory.CreateQuery<GetPaymentByRideIdQuery>(request);
            var payment = await _mediator.Send(paymentQuery);

            if (payment == null)
            {
                return new ResponseDTO<UpdatePaymentResponseDTO>
                {
                    Info = PaymentErrors.ID_NOT_FOUND.FormatMessage(("PaymentId", request.PaymentId))
                };
            }

            if (payment.Status != PaymentStatusEnum.Processing)
            {
                return new ResponseDTO<UpdatePaymentResponseDTO>
                {
                    Info = PaymentErrors.STATUS_INVALID.FormatMessage(("Status", payment.Status.ToString()))
                };
            }

            if (payment.TransactionId == null)
            {
                return new ResponseDTO<UpdatePaymentResponseDTO>
                {
                    Info = PaymentErrors.TRANSACTION_ID_EMPTY
                };
            }

            var result = await _transactionService.CaptureTransactionAsync(payment.PaymentMethod.PaymentMethodId, payment.TransactionId);
            var command = new UpdatePaymentCommand
            {
                PaymentId = payment.PaymentId,
                Status = PaymentStatusEnum.Completed
            };

            var updatedPayment = await _mediator.Send(command);
            await _mediator.Send(new SaveChangesCommand());

            return new ResponseDTO<UpdatePaymentResponseDTO> { Data = updatedPayment };
        }
        catch (Exception ex)
        {
            await HandlePaymentErrorAsync(request.PaymentId);
            return new ResponseDTO<UpdatePaymentResponseDTO>
            {
                Info = new ResponseInfo { Code = "CAPTURE_PAYMENT_ERROR", Message = ex.Message }
            };
        }
    }

    private async Task HandlePaymentErrorAsync(Guid paymentId)
    {
        try
        {
            var command = new UpdatePaymentCommand
            {
                PaymentId = paymentId,
                Status = PaymentStatusEnum.Failed
            };

            await _mediator.Send(command);
            await _mediator.Send(new SaveChangesCommand());
        }
        catch (Exception ex)
        {
            // Log the error message
            Console.WriteLine($"Error updating payment status: {ex.Message}");
        }
    }
}
