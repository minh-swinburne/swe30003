using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using frontend.Models;

namespace frontend.Services.Interfaces
{
    public interface IUserService
    {
        // Create a new user
        Task<CreateUserResponseDTO> CreateUserAsync(CreateUserRequestDTO request);

        // Get user by ID
        Task<GetUserResponseDTO> GetUserByIdAsync(Guid userId);

        // Get user by email
        Task<GetUserResponseDTO> GetUserByEmailAsync(string email);

        // Get user by phone
        Task<GetUserResponseDTO> GetUserByPhoneAsync(string phone);

        // Update user
        Task<UpdateUserResponseDTO> UpdateUserAsync(UpdateUserRequestDTO request);

        // Delete user
        Task<DeleteUserResponseDTO> DeleteUserAsync(Guid userId);

        // List users with pagination and filtering
        Task<PaginatedResponse<ListUserResponseDTO>> ListUsersAsync(ListUserRequestDTO request);

        // Authentication methods (these would connect to your auth endpoints)
        Task<string> LoginAsync(string email, string password);
        Task<bool> LogoutAsync();
        Task<bool> ChangePasswordAsync(Guid userId, string currentPassword, string newPassword);
    }
}

