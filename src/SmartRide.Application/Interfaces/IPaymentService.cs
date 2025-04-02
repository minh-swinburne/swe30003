using SmartRide.Application.DTOs;
using SmartRide.Application.DTOs.Payments;

namespace SmartRide.Application.Interfaces;

public interface IPaymentService
{
    Task<ResponseDTO<ExecutePaymentResponseDTO>> ExecutePaymentAsync(ExecutePaymentRequestDTO request);
}

