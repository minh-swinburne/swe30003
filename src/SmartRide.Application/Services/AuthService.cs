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

public class AuthService(IUserService userService, IJwtService jwtService, IPasswordHasher<User> passwordHasher, IMapper mapper) : IAuthService
{
    private readonly IUserService _userService = userService;
    private readonly IJwtService _jwtService = jwtService;
    private readonly IPasswordHasher<User> _passwordHasher = passwordHasher;
    private readonly IMapper _mapper = mapper;

    public async Task<ResponseDTO<string>> LoginAsync(LoginRequestDTO request)
    {
        try
        {
            var userResponse = await _userService.GetUserByEmailAsync(new GetUserByEmailRequestDTO { Email = request.Email });

            if (userResponse.Data == null)
            {
                return new ResponseDTO<string>
                {
                    Info = new ResponseInfo { Code = "USER_NOT_FOUND", Message = "User not found." }
                };
            }

            var user = _mapper.Map<User>(userResponse.Data);
            var passwordVerificationResult = _passwordHasher.VerifyHashedPassword(user, user.Password!, request.Password);

            if (passwordVerificationResult != PasswordVerificationResult.Success)
            {
                return new ResponseDTO<string>
                {
                    Data = null,
                    Info = new ResponseInfo { Code = "INVALID_PASSWORD", Message = "Invalid password." }
                };
            }

            var roles = user.Roles.Select(r => r.Name).ToList();

            var token = _jwtService.GenerateToken([
                new Claim("sub", user.Id.ToString()),
                new Claim("email", user.Email),
                new Claim("phone", user.Phone),
                new Claim("firstName", user.FirstName),
                new Claim("lastName", user.LastName ?? ""),
                new Claim("roles", string.Join(",", roles)),
                // new Claim("iat", DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString())
            ]);

            return new ResponseDTO<string> { Data = token };
        }
        catch (Exception ex)
        {
            return new ResponseDTO<string>
            {
                Info = new ResponseInfo { Code = "LOGIN_ERROR", Message = ex.Message }
            };
        }
    }

    public async Task<ResponseDTO<CreateUserResponseDTO>> RegisterAsync(CreateUserRequestDTO request)
    {
        try
        {
            var userResponse = await _userService.CreateUserAsync(request);
            return new ResponseDTO<CreateUserResponseDTO> { Data = userResponse.Data };
        }
        catch (Exception ex)
        {
            return new ResponseDTO<CreateUserResponseDTO>
            {
                Info = new ResponseInfo { Code = "REGISTER_ERROR", Message = ex.Message }
            };
        }
    }

    public ResponseDTO<List<Claim>> ValidateToken(ValidateTokenRequestDTO request)
    {
        try
        {
            var principal = _jwtService.ValidateToken(request.Token);

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
