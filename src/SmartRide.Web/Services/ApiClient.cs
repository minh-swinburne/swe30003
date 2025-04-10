using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using SmartRide.Web.Services.Interfaces;

namespace SmartRide.Web.Services
{
    public class ApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly ITokenService _tokenService;

        // API domain services
        public IUserService User { get; }
        public IAuthService Auth { get; }

        public ApiClient(
            HttpClient httpClient,
            IUserService userService,
            ITokenService tokenService,
            IAuthService authService)
        {
            _httpClient = httpClient;
            _tokenService = tokenService;

            // Set base address
            _httpClient.BaseAddress = new Uri("http://localhost:5275/api/v1/");

            // Assign services
            User = userService;
            Auth = authService;

            // Initialize the client
            InitializeClient();
        }

        private async void InitializeClient()
        {
            // Set default headers
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // Get token from local storage
            var token = await _tokenService.GetTokenAsync();
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                // Validate the token
                var isValid = await _tokenService.ValidateTokenAsync(token);
                if (!isValid)
                {
                    // If token is invalid, remove it
                    await _tokenService.RemoveTokenAsync();
                }
            }
        }

        public async Task<bool> IsAuthenticated()
        {
            return await _tokenService.IsTokenValidAsync();
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
