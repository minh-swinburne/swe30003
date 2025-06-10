using Microsoft.Extensions.Options;
using SmartRide.Domain.Enums;
using SmartRide.Domain.Interfaces;
using SmartRide.Infrastructure.Settings;

namespace SmartRide.Infrastructure.Services.Transaction;

public class CreditCardProcessor(IOptions<TransactionSettings> txSettings) : ITransactionProcessor
{
    private readonly CreditCardSettings _ccSettings = txSettings.Value.CreditCard;
    public PaymentMethodEnum PaymentMethod => PaymentMethodEnum.CreditCard;

    public Task<(string, string)> CreateTransactionAsync(
        decimal amount,
        string currency,
        RideTypeEnum rideType,
        VehicleTypeEnum vehicleType,
        string pickupAddress,
        string destinationAddress
        )
    {
        // Implement credit card transaction creation logic here
        throw new NotImplementedException();
    }

    public Task<bool> CaptureTransactionAsync(string transactionId)
    {
        // Implement credit card transaction capture logic here
        throw new NotImplementedException();
    }

    public Task<bool> RefundTransactionAsync(string transactionId)
    {
        // Implement credit card transaction refund logic here
        throw new NotImplementedException();
    }
}
