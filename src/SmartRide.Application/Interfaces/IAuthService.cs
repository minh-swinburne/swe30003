using System.Security.Claims;
using SmartRide.Application.DTOs;
using SmartRide.Application.DTOs.Auth;
using SmartRide.Application.DTOs.Users;

namespace SmartRide.Application.Interfaces;

public interface IAuthService
{
    Task<ResponseDTO<AuthResponseDTO>> LoginAsync(LoginRequestDTO request);
    Task<ResponseDTO<AuthResponseDTO>> RegisterAsync(CreateUserRequestDTO request);
    ResponseDTO<ValidateTokenResponseDTO> ValidateToken(ValidateTokenRequestDTO request);
}
