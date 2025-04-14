using MediatR;
using SmartRide.Application.Commands;
using SmartRide.Application.Commands.Payments;
using SmartRide.Application.DTOs;
using SmartRide.Application.DTOs.Payments;
using SmartRide.Application.Factories;
using SmartRide.Application.Interfaces;
using SmartRide.Application.Queries.Payments;
using SmartRide.Common.Responses;
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

    // public async Task<ResponseDTO<UpdatePaymentResponseDTO>> RequestPaymentAsync(UpdatePaymentRequestDTO request)
    // {
    //     try
    //     {
    //         var command = MediatRFactory.CreateCommand<CreatePaymentCommand>(request);
    //         var result = await _mediator.Send(command);
    //         await _mediator.Send(new SaveChangesCommand());
    //         return new ResponseDTO<UpdatePaymentResponseDTO> { Data = result };
    //     }
    //     catch (Exception ex)
    //     {
    //         return new ResponseDTO<UpdatePaymentResponseDTO>
    //         {
    //             Info = new ResponseInfo { Code = "CREATE_PAYMENT_ERROR", Message = ex.Message }
    //         };
    //     }
    // }

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
}
