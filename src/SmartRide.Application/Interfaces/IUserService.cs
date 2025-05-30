﻿using SmartRide.Application.DTOs;
using SmartRide.Application.DTOs.Users;

namespace SmartRide.Application.Interfaces;

public interface IUserService
{
    Task<ListResponseDTO<ListUserResponseDTO>> ListUsersAsync(ListUserRequestDTO request);
    Task<ResponseDTO<GetUserResponseDTO>> GetCurrentUserAsync(GetCurrentUserRequestDTO request);
    Task<ResponseDTO<GetUserResponseDTO>> GetUserByIdAsync(GetUserByIdRequestDTO request);
    Task<ResponseDTO<GetUserResponseDTO>> GetUserByEmailAsync(GetUserByEmailRequestDTO request);
    Task<ResponseDTO<GetUserResponseDTO>> GetUserByPhoneAsync(GetUserByPhoneRequestDTO request);
    Task<ResponseDTO<CreateUserResponseDTO>> CreateUserAsync(CreateUserRequestDTO request);
    Task<ResponseDTO<UpdateUserResponseDTO>> UpdateUserAsync(UpdateUserRequestDTO request);
    Task<ResponseDTO<DeleteUserResponseDTO>> DeleteUserAsync(DeleteUserRequestDTO request);
}
