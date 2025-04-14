namespace SmartRide.Infrastructure.Settings;

public class CreditCardSettings
{
    public required string CardNumber { get; set; }
    public required string ExpirationDate { get; set; } // Format: MM/YY
    public required string Cvc { get; set; }
    public required string CardHolderName { get; set; }
}

public class PayPalSettings
{
    public required string ClientId { get; set; }
    public required string ClientSecret { get; set; }
    public required bool SandboxMode { get; set; } = true;
    // public required string ApiUrl { get; set; }
    // public required string ReturnUrl { get; set; }
    // public required string CancelUrl { get; set; }
}

public class TransactionSettings
{
    public required string DefaultCurrency { get; set; }
    public required CreditCardSettings CreditCard { get; set; }
    public required PayPalSettings PayPal { get; set; }
}
