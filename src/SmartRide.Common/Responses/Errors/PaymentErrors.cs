using SmartRide.Common.Constants;

namespace SmartRide.Common.Responses.Errors;

public class PaymentErrors
{
    public static readonly string Module = "Payment";

    public static readonly ResponseInfo ID_EMPTY = new()
    {
        Code = $"{Module}.ID_EMPTY",
        Message = "Payment ID cannot be empty."
    };

    public static readonly ResponseInfo ID_INVALID = new()
    {
        Code = $"{Module}.ID_INVALID",
        Message = "Payment ID must be a valid GUID."
    };

    public static readonly ResponseInfo ID_NOT_FOUND = new()
    {
        Code = $"{Module}.ID_NOT_FOUND",
        Message = "Payment with ID {PaymentId} not found."
    };

    public static readonly ResponseInfo AMOUNT_INVALID = new()
    {
        Code = $"{Module}.AMOUNT_INVALID",
        Message = $"Amount must be between {PaymentConstants.MinAmount} and {PaymentConstants.MaxAmount}."
    };

    public static readonly ResponseInfo METHOD_INVALID = new()
    {
        Code = $"{Module}.METHOD_INVALID",
        Message = "Payment method is invalid."
    };

    public static readonly ResponseInfo STATUS_INVALID = new()
    {
        Code = $"{Module}.STATUS_INVALID",
        Message = "Payment status is invalid."
    };

    public static readonly ResponseInfo TIME_IN_FUTURE = new()
    {
        Code = $"{Module}.TIME_IN_FUTURE",
        Message = "Payment time must not be in the future."
    };
}
