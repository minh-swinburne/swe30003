namespace SmartRide.Application.DTOs.Payments
{
    public class ExecutePaymentResponseDTO : BaseDTO
    {
        public required bool Success { get; set; }
        public string? Message { get; set; }
    }
}
