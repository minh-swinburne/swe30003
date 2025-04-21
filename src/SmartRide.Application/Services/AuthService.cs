using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using SmartRide.Application.DTOs;
using SmartRide.Application.DTOs.Auth;
using SmartRide.Application.DTOs.Users;
using SmartRide.Application.Interfaces;
using SmartRide.Common.Responses;
using SmartRide.Domain.Entities.Base;
using SmartRide.Domain.Interfaces;

namespace SmartRide.Application.Services;

public class AuthService(IPasswordHasher<User> passwordHasher, IUserService userService, IJwtService jwtService) : IAuthService
{
    private readonly IPasswordHasher<User> _passwordHasher = passwordHasher;
    private readonly IUserService _userService = userService;
    private readonly IJwtService _jwtService = jwtService;

    private string GenerateAccessToken(GetUserResponseDTO user)
    {
        return _jwtService.GenerateToken([
            new Claim("sub", user.UserId.ToString()),
            new Claim("email", user.Email),
            new Claim("phone", user.Phone),
            new Claim("firstName", user.FirstName),
            new Claim("lastName", user.LastName ?? ""),
            new Claim("roles", string.Join(",", user.Roles.Select(r => r.Name))),
            // new Claim("iat", DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString())
        ]);
    }

    public async Task<ResponseDTO<AuthResponseDTO>> LoginAsync(LoginRequestDTO request)
    {
        try
        {
            var userResponse = await _userService.GetUserByEmailAsync(new GetUserByEmailRequestDTO { Email = request.Email });
            var user = userResponse.Data;

            if (user == null)
            {
                return new ResponseDTO<AuthResponseDTO>
                {
                    Info = new ResponseInfo { Code = "USER_NOT_FOUND", Message = "User not found." }
                };
            }

            var passwordVerificationResult = _passwordHasher.VerifyHashedPassword(null!, user.Password!, request.Password);

            if (passwordVerificationResult != PasswordVerificationResult.Success)
            {
                return new ResponseDTO<AuthResponseDTO>
                {
                    Data = null,
                    Info = new ResponseInfo { Code = "INVALID_PASSWORD", Message = "Invalid password." }
                };
            }

            var token = GenerateAccessToken(user);

            return new ResponseDTO<AuthResponseDTO>
            {
                Data = new AuthResponseDTO
                {
                    AccessToken = token,
                    // RefreshToken = null, // TODO: Implement refresh token logic
                }
            };
        }
        catch (Exception ex)
        {
            return new ResponseDTO<AuthResponseDTO>
            {
                Info = new ResponseInfo { Code = "LOGIN_ERROR", Message = ex.Message }
            };
        }
    }

    public async Task<ResponseDTO<AuthResponseDTO>> RegisterAsync(CreateUserRequestDTO request)
    {
        try
        {
            var userResponse = await _userService.CreateUserAsync(request);
            var user = await _userService.GetUserByIdAsync(new GetUserByIdRequestDTO { UserId = userResponse.Data!.UserId });

            return await LoginAsync(new LoginRequestDTO
            {
                Email = user.Data!.Email,
                Password = user.Data!.Password!
            });
        }
        catch (Exception ex)
        {
            return new ResponseDTO<AuthResponseDTO>
            {
                Info = new ResponseInfo { Code = "REGISTER_ERROR", Message = ex.Message }
            };
        }
    }

    public ResponseDTO<List<Claim>> ValidateToken(ValidateTokenRequestDTO request)
    {
        try
        {
            var principal = _jwtService.ValidateToken(request.AccessToken);

            if (principal == null)
            {
                return new ResponseDTO<List<Claim>>
                {
                    Info = new ResponseInfo { Code = "INVALID_TOKEN", Message = "Token validation failed." }
                };
            }

            var claims = principal.Claims.ToList();
            return new ResponseDTO<List<Claim>> { Data = claims };
        }
        catch (Exception ex)
        {
            return new ResponseDTO<List<Claim>>
            {
                Info = new ResponseInfo { Code = "TOKEN_VALIDATION_ERROR", Message = ex.Message }
            };
        }
    }
}
