using SmartRide.Domain.Enums;

namespace SmartRide.Domain.Interfaces;

public interface ITransactionProcessor
{
    PaymentMethodEnum PaymentMethod { get; }
    Task<(string, string)> CreateTransactionAsync(
        decimal amount,
        string currency,
        RideTypeEnum rideType,
        VehicleTypeEnum vehicleType,
        string pickupAddress,
        string destinationAddress
        );
    Task<bool> CaptureTransactionAsync(string transactionId);
    Task<bool> RefundTransactionAsync(string transactionId);
}
