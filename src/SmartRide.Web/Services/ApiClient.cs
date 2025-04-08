using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using SmartRide.Web.Services.Interfaces;

namespace SmartRide.Web.Services
{
    public class ApiClient
    {
        private readonly HttpClient _httpClient;

        // API domain services
        public IUserService User { get; }

        public ApiClient(
            HttpClient httpClient,
            IUserService userService)
        {
            _httpClient = httpClient;

            // Set base address
            _httpClient.BaseAddress = new Uri("http://localhost:5275/api/v1/");

            // Assign services
            User = userService;

            // Initialize the client
            InitializeClient();
        }

        private void InitializeClient()
        {
            // Set default headers
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // Get token from local storage if you have a mechanism for that
            var token = GetStoredToken();
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }

        private string GetStoredToken()
        {
            // Implement your token storage mechanism here
            // For example, you might get it from a cookie, local storage, or session storage
            return null;
        }
    }

    public class ApiException : Exception
    {
        public int StatusCode { get; }
        public string Response { get; }

        public ApiException(string message, int statusCode, string response, Exception innerException)
            : base(message, innerException)
        {
            StatusCode = statusCode;
            Response = response;
        }
    }
}
