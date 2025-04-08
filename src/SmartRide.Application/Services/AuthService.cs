using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using SmartRide.Application.DTOs;
using SmartRide.Application.DTOs.Auth;
using SmartRide.Application.DTOs.Users;
using SmartRide.Application.Interfaces;
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
        var userResponse = await _userService.GetUserByEmailAsync(new GetUserByEmailRequestDTO { Email = request.Email });

        if (userResponse.Data == null)
        {
            return new ResponseDTO<string> { Data = "" };
        }

        var user = _mapper.Map<User>(userResponse.Data);
        var passwordVerificationResult = _passwordHasher.VerifyHashedPassword(user, user.Password!, request.Password);

        if (passwordVerificationResult != PasswordVerificationResult.Success)
        {
            return new ResponseDTO<string> { Data = "" };
        }

        var roles = user.Roles.Select(r => r.Name).ToList();

        var token = _jwtService.GenerateToken(
            [
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.FirstName),
                new Claim(ClaimTypes.Role, string.Join(",", roles))
            ],
            expiration: TimeSpan.FromMinutes(60)
        );

        return new ResponseDTO<string> { Data = "" };
    }

    public async Task<ResponseDTO<CreateUserResponseDTO>> RegisterAsync(CreateUserRequestDTO request)
    {
        var userResponse = await _userService.CreateUserAsync(request);
        return new ResponseDTO<CreateUserResponseDTO> { Data = userResponse.Data };
    }

    public ResponseDTO<List<Claim>> ValidateToken(ValidateTokenRequestDTO request)
    {
        var principal = _jwtService.ValidateToken(request.Token);

        if (principal == null)
        {
            return new ResponseDTO<List<Claim>> { Data = [] };
        }

        var claims = principal.Claims.ToList();
        return new ResponseDTO<List<Claim>> { Data = claims };
    }
}
