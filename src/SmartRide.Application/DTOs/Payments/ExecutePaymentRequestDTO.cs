namespace SmartRide.Application.DTOs.Payments
{
    public class ExecutePaymentRequestDTO : BaseDTO
    {
        public required decimal Amount { get; set; }
        public required string Currency { get; set; }
    }
}
