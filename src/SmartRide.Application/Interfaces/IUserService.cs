using SmartRide.Application.DTOs;
using SmartRide.Application.DTOs.User;

namespace SmartRide.Application.Interfaces;

public interface IUserService
{
    Task<ListResponseDTO<ListUserResponseDTO>> GetAllUsersAsync();
    Task<ResponseDTO<GetUserResponseDTO>> GetUserByIdAsync(GetUserRequestDTO request);
    Task<ResponseDTO<CreateUserResponseDTO>> CreateUserAsync(CreateUserRequestDTO request);
    //Task<User> UpdateUserAsync(User user);
    //Task<User> DeleteUserAsync(string id);
}
