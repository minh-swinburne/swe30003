using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SmartRide.Web.Models;

namespace SmartRide.Web.Services.Interfaces
{
    public interface IUserService
    {
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
    }
}
