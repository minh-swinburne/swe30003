using System.Security.Claims;
using SmartRide.Application.DTOs;
using SmartRide.Application.DTOs.Auth;
using SmartRide.Application.DTOs.Users;

namespace SmartRide.Application.Interfaces;

public interface IAuthService
{
    Task<ResponseDTO<string>> LoginAsync(LoginRequestDTO request);
    Task<ResponseDTO<CreateUserResponseDTO>> RegisterAsync(CreateUserRequestDTO request);
    ResponseDTO<List<Claim>> ValidateToken(ValidateTokenRequestDTO request);
}
