using SmartRide.Application.DTOs;
using SmartRide.Application.DTOs.Payments;

namespace SmartRide.Application.Interfaces;

public interface IPaymentService
{
    Task<ListResponseDTO<ListPaymentResponseDTO>> ListPaymentsAsync(ListPaymentRequestDTO request);
    Task<ResponseDTO<GetPaymentResponseDTO>> GetPaymentByIdAsync(GetPaymentByIdRequestDTO request);
    Task<ResponseDTO<UpdatePaymentResponseDTO>> UpdatePaymentAsync(UpdatePaymentRequestDTO request);
    Task<ResponseDTO<RequestPaymentResponseDTO>> RequestPaymentAsync(GetPaymentByIdRequestDTO request);
    Task<ResponseDTO<UpdatePaymentResponseDTO>> CapturePaymentAsync(GetPaymentByIdRequestDTO request);
}
