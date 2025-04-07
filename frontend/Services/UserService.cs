using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using frontend.Models;
using frontend.Services.Interfaces;

namespace frontend.Services
{
    public class UserService : IUserService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "users";

        public UserService(HttpClient httpClient)
        {
            _httpClient = httpClient;

            // Ensure the base address is set
            if (_httpClient.BaseAddress == null)
            {
                _httpClient.BaseAddress = new Uri("http://localhost:5275/api/v1/");
            }
        }

        public async Task<CreateUserResponseDTO> CreateUserAsync(CreateUserRequestDTO request)
        {
            var response = await _httpClient.PostAsJsonAsync($"{BaseUrl}", request);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new ApiException(
                    $"Failed to create user. Status code: {(int)response.StatusCode}",
                    (int)response.StatusCode,
                    errorContent,
                    null
                );
            }

            var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<CreateUserResponseDTO>>();

            if (apiResponse == null || !apiResponse.Success || apiResponse.Data == null)
            {
                throw new ApiException(
                    "Failed to create user. Invalid response from server.",
                    (int)response.StatusCode,
                    apiResponse?.Message ?? "Unknown error",
                    null
                );
            }

            return apiResponse.Data;
        }

        public async Task<GetUserResponseDTO> GetUserByIdAsync(Guid userId)
        {
            var response = await _httpClient.GetAsync($"{BaseUrl}/{userId}");

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new ApiException(
                    $"Failed to get user. Status code: {(int)response.StatusCode}",
                    (int)response.StatusCode,
                    errorContent,
                    null
                );
            }

            var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<GetUserResponseDTO>>();

            if (apiResponse == null || !apiResponse.Success || apiResponse.Data == null)
            {
                throw new ApiException(
                    "Failed to get user. Invalid response from server.",
                    (int)response.StatusCode,
                    apiResponse?.Message ?? "Unknown error",
                    null
                );
            }

            return apiResponse.Data;
        }

        public async Task<GetUserResponseDTO> GetUserByEmailAsync(string email)
        {
            var request = new GetUserByEmailRequestDTO { Email = email };
            var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"{BaseUrl}/by-email", content);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new ApiException(
                    $"Failed to get user by email. Status code: {(int)response.StatusCode}",
                    (int)response.StatusCode,
                    errorContent,
                    null
                );
            }

            var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<GetUserResponseDTO>>();

            if (apiResponse == null || !apiResponse.Success || apiResponse.Data == null)
            {
                throw new ApiException(
                    "Failed to get user by email. Invalid response from server.",
                    (int)response.StatusCode,
                    apiResponse?.Message ?? "Unknown error",
                    null
                );
            }

            return apiResponse.Data;
        }

        public async Task<GetUserResponseDTO> GetUserByPhoneAsync(string phone)
        {
            var request = new GetUserByPhoneRequestDTO { Phone = phone };
            var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"{BaseUrl}/by-phone", content);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new ApiException(
                    $"Failed to get user by phone. Status code: {(int)response.StatusCode}",
                    (int)response.StatusCode,
                    errorContent,
                    null
                );
            }

            var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<GetUserResponseDTO>>();

            if (apiResponse == null || !apiResponse.Success || apiResponse.Data == null)
            {
                throw new ApiException(
                    "Failed to get user by phone. Invalid response from server.",
                    (int)response.StatusCode,
                    apiResponse?.Message ?? "Unknown error",
                    null
                );
            }

            return apiResponse.Data;
        }

        public async Task<UpdateUserResponseDTO> UpdateUserAsync(UpdateUserRequestDTO request)
        {
            var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"{BaseUrl}/{request.UserId}", content);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new ApiException(
                    $"Failed to update user. Status code: {(int)response.StatusCode}",
                    (int)response.StatusCode,
                    errorContent,
                    null
                );
            }

            var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<UpdateUserResponseDTO>>();

            if (apiResponse == null || !apiResponse.Success || apiResponse.Data == null)
            {
                throw new ApiException(
                    "Failed to update user. Invalid response from server.",
                    (int)response.StatusCode,
                    apiResponse?.Message ?? "Unknown error",
                    null
                );
            }

            return apiResponse.Data;
        }

        public async Task<DeleteUserResponseDTO> DeleteUserAsync(Guid userId)
        {
            var request = new DeleteUserRequestDTO { UserId = userId };
            var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

            var requestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Delete,
                RequestUri = new Uri($"{_httpClient.BaseAddress}{BaseUrl}/{userId}"),
                Content = content
            };

            var response = await _httpClient.SendAsync(requestMessage);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new ApiException(
                    $"Failed to delete user. Status code: {(int)response.StatusCode}",
                    (int)response.StatusCode,
                    errorContent,
                    null
                );
            }

            var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<DeleteUserResponseDTO>>();

            if (apiResponse == null || !apiResponse.Success || apiResponse.Data == null)
            {
                throw new ApiException(
                    "Failed to delete user. Invalid response from server.",
                    (int)response.StatusCode,
                    apiResponse?.Message ?? "Unknown error",
                    null
                );
            }

            return apiResponse.Data;
        }

        public async Task<PaginatedResponse<ListUserResponseDTO>> ListUsersAsync(ListUserRequestDTO request)
        {
            var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"{BaseUrl}/list", content);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new ApiException(
                    $"Failed to list users. Status code: {(int)response.StatusCode}",
                    (int)response.StatusCode,
                    errorContent,
                    null
                );
            }

            var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<PaginatedResponse<ListUserResponseDTO>>>();

            if (apiResponse == null || !apiResponse.Success || apiResponse.Data == null)
            {
                throw new ApiException(
                    "Failed to list users. Invalid response from server.",
                    (int)response.StatusCode,
                    apiResponse?.Message ?? "Unknown error",
                    null
                );
            }

            return apiResponse.Data;
        }

        public async Task<string> LoginAsync(string email, string password)
        {
            var loginRequest = new
            {
                Email = email,
                Password = password
            };

            var content = new StringContent(JsonSerializer.Serialize(loginRequest), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("auth/login", content);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new ApiException(
                    $"Login failed. Status code: {(int)response.StatusCode}",
                    (int)response.StatusCode,
                    errorContent,
                    null
                );
            }

            var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<string>>();

            if (apiResponse == null || !apiResponse.Success)
            {
                throw new ApiException(
                    "Login failed. Invalid response from server.",
                    (int)response.StatusCode,
                    apiResponse?.Message ?? "Unknown error",
                    null
                );
            }

            // Return the token
            return apiResponse.Data ?? string.Empty;
        }

        public async Task<bool> LogoutAsync()
        {
            var response = await _httpClient.PostAsync("auth/logout", null);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new ApiException(
                    $"Logout failed. Status code: {(int)response.StatusCode}",
                    (int)response.StatusCode,
                    errorContent,
                    null
                );
            }

            var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<bool>>();

            return apiResponse?.Success ?? false;
        }

        public async Task<bool> ChangePasswordAsync(Guid userId, string currentPassword, string newPassword)
        {
            var changePasswordRequest = new
            {
                UserId = userId,
                CurrentPassword = currentPassword,
                NewPassword = newPassword
            };

            var content = new StringContent(JsonSerializer.Serialize(changePasswordRequest), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("auth/change-password", content);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new ApiException(
                    $"Change password failed. Status code: {(int)response.StatusCode}",
                    (int)response.StatusCode,
                    errorContent,
                    null
                );
            }

            var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<bool>>();

            return apiResponse?.Success ?? false;
        }
    }
}

