using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SmartRide.Infrastructure.Services.Payment;
using SmartRide.Infrastructure.Settings;
using System.Net.Http.Headers;
using System.Text;

namespace SmartRide.Infrastructure.Strategies
{
    public class PayPalStrategy : IPaymentGatewayStrategy
    {
        private readonly string _apiEndpoint;
        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly HttpClient _httpClient;
        private readonly ILogger<PayPalStrategy> _logger;

        // Inject PaymentSettings via IOptions to access PayPal configuration
        public PayPalStrategy(IOptions<PaymentGatewaySettings> paymentSettings, HttpClient httpClient, ILogger<PayPalStrategy> logger)
        {
            var payPalSettings = paymentSettings.Value.PayPal;
            _apiEndpoint = payPalSettings.ApiEndPoint;
            _clientId = payPalSettings.ClientId;
            _clientSecret = payPalSettings.ClientSecret;
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<bool> ProcessPaymentAsync(decimal amount, string currency)
        {
            try
            {
                // Prepare PayPal API request with the correct API endpoint
                var request = new HttpRequestMessage(HttpMethod.Post, _apiEndpoint)
                {
                    Content = new StringContent("{ \"amount\": " + amount + ", \"currency\": \"" + currency + "\" }")
                };

                // Set the authorization header for PayPal API using client credentials
                var authValue = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{_clientId}:{_clientSecret}"));
                request.Headers.Authorization = new AuthenticationHeaderValue("Basic", authValue);

                // Send the request and capture the response
                var response = await _httpClient.SendAsync(request);

                // Log and handle the response
                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Payment successfully processed.");
                    return true;
                }
                else
                {
                    _logger.LogError("Error processing payment: {0}", response.StatusCode);
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing payment");
                return false;
            }
        }
    }
}
