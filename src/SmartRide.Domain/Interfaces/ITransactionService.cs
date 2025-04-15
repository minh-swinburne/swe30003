using SmartRide.Domain.Enums;

namespace SmartRide.Domain.Interfaces;

public interface ITransactionService
{
    Task<(string, string)> CreateTransactionAsync(
        PaymentMethodEnum paymentMethod,
        decimal amount,
        string currency,
        RideTypeEnum rideType,
        VehicleTypeEnum vehicleType,
        string pickupAddress,
        string destinationAddress
        );
    Task<bool> CaptureTransactionAsync(PaymentMethodEnum paymentMethod, string transactionId);
    Task<bool> RefundTransactionAsync(PaymentMethodEnum paymentMethod, string transactionId);
}
