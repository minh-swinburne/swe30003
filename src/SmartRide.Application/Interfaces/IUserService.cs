using SmartRide.Application.DTOs;
using SmartRide.Application.DTOs.User;
using SmartRide.Domain.Entities;
using SmartRide.Domain.Interfaces;

namespace SmartRide.Application.Interfaces;

public interface IUserService
{
    Task<ListResponseDTO<ListUserResponseDTO>> GetAllUsersAsync(ListUserRequestDTO request);
    Task<ResponseDTO<GetUserResponseDTO>> GetUserByIdAsync(GetUserRequestDTO request);
    Task<ResponseDTO<CreateUserResponseDTO>> CreateUserAsync(CreateUserRequestDTO request);
    //Task<User> UpdateUserAsync(User user);
    //Task<User> DeleteUserAsync(string id);
}
