using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

class Program
{
    static async Task Main(string[] args)
    {
        // Load configuration
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetParent(AppContext.BaseDirectory)?.Parent?.Parent?.Parent?.FullName ?? Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        // Load PayPal configuration
        var payPalConfig = configuration.GetSection("PayPal");
        string clientId = payPalConfig["ClientId"];
        string clientSecret = payPalConfig["ClientSecret"];
        string payPalBaseUrl = payPalConfig["Mode"] == "sandbox"
            ? "https://api-m.sandbox.paypal.com"
            : "https://api-m.paypal.com";

        // Setup DI for HttpClient
        var services = new ServiceCollection();
        services.AddHttpClient();
        var provider = services.BuildServiceProvider();
        var httpClientFactory = provider.GetRequiredService<IHttpClientFactory>();
        var httpClient = httpClientFactory.CreateClient();

        try
        {
            // Get PayPal Access Token
            string auth = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{clientId}:{clientSecret}"));
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", auth);

            var tokenResponse = await httpClient.PostAsync($"{payPalBaseUrl}/v1/oauth2/token",
                new FormUrlEncodedContent(new[] { new KeyValuePair<string, string>("grant_type", "client_credentials") }));

            tokenResponse.EnsureSuccessStatusCode();
            var tokenJson = await tokenResponse.Content.ReadAsStringAsync();
            using var jsonDoc = JsonDocument.Parse(tokenJson);
            string accessToken = jsonDoc.RootElement.GetProperty("access_token").GetString();

            // Print the PayPal access token
            Console.WriteLine($"PayPal Access Token: {accessToken}");
            Console.WriteLine("--------");

            // Simulate a payment transaction with PayPal API
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var orderPayload = new
            {
                intent = "CAPTURE",
                purchase_units = new[]
                {
                    new
                    {
                        amount = new
                        {
                            currency_code = "USD",
                            value = "10.00"
                        }
                    }
                },
                application_context = new
                {
                    return_url = "http://your-return-url.com", // Placeholder URL
                    cancel_url = "http://your-cancel-url.com"  // Placeholder URL
                }
            };

            var orderContent = new StringContent(JsonSerializer.Serialize(orderPayload), Encoding.UTF8, "application/json");
            var orderResponse = await httpClient.PostAsync($"{payPalBaseUrl}/v2/checkout/orders", orderContent);
            string orderJson = await orderResponse.Content.ReadAsStringAsync();

            // Save orderJson to a JSON file in the root folder
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "orderResponse.json");
            await File.WriteAllTextAsync(filePath, orderJson);

            // Check the status of the order
            if (orderResponse.IsSuccessStatusCode)
            {
                using var orderDoc = JsonDocument.Parse(orderJson);
                string orderId = orderDoc.RootElement.GetProperty("id").GetString();
                string responseCode = orderDoc.RootElement.GetProperty("status").GetString(); // Assuming response_code is in the status property

                Console.WriteLine($"Order ID: {orderId}");
                Console.WriteLine($"Response Code: {responseCode}"); // Print the response code

                // Get approval link
                var approvalLink = orderDoc.RootElement
                    .GetProperty("links")
                    .EnumerateArray()
                    .First(link => link.GetProperty("rel").GetString() == "approve")
                    .GetProperty("href")
                    .GetString();

                Console.WriteLine($"Approval Link: {approvalLink}");
                Console.WriteLine("Please approve the payment in the browser and press Enter to continue...");
                Console.ReadLine();
                Console.WriteLine("--------");

                // Capture the payment
                var captureResponse = await httpClient.PostAsync(
                    $"{payPalBaseUrl}/v2/checkout/orders/{orderId}/capture",
                    new StringContent("{}", Encoding.UTF8, "application/json") // Empty JSON payload
                );

                string captureJson = await captureResponse.Content.ReadAsStringAsync();
                using var captureDoc = JsonDocument.Parse(captureJson);

                // Print full JSON response for debugging
                Console.WriteLine($"Capture Response: {captureJson}");

                // Save captureJson to a JSON file in the root folder
                string captureFilePath = Path.Combine(Directory.GetCurrentDirectory(), "captureResponse.json");
                await File.WriteAllTextAsync(captureFilePath, captureJson);

                if (captureDoc.RootElement.TryGetProperty("status", out JsonElement statusElement))
                {
                    string captureStatus = statusElement.GetString();
                    Console.WriteLine($"Payment Capture Status: {captureStatus}");
                }
                else
                {
                    Console.WriteLine("Error: 'status' key not found in response.");
                }
            }
            else
            {
                // In case of failure, log the error
                Console.WriteLine($"Error: {orderJson}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}
