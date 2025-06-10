using Microsoft.Extensions.Options;
using SmartRide.Web.Services.Interfaces;
using SmartRide.Web.Settings;
using System.Net.Http.Headers;

namespace SmartRide.Web.Services
{
    public class ApiClient
    {
        private readonly ApiSettings _apiSettings;
        private readonly HttpClient _httpClient;
        private readonly ITokenService _tokenService;

        // API domain services
        public IUserService User { get; }
        public IAuthService Auth { get; }

        public ApiClient(
            HttpClient httpClient,
            IOptions<ApiSettings> apiSettings,
            IUserService userService,
            ITokenService tokenService,
            IAuthService authService)
        {
            _apiSettings = apiSettings.Value;
            _httpClient = httpClient;
            _tokenService = tokenService;

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

    public class ApiException(string message, int statusCode, string response, Exception innerException) : Exception(message, innerException)
    {
        public int StatusCode { get; } = statusCode;
        public string Response { get; } = response;
    }
}
