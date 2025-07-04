using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Identity;
using SmartRide.Application.Commands;
using SmartRide.Application.Commands.Users;
using SmartRide.Application.DTOs;
using SmartRide.Application.DTOs.Auth;
using SmartRide.Application.DTOs.Users;
using SmartRide.Application.Factories;
using SmartRide.Application.Interfaces;
using SmartRide.Application.Queries.Users;
using SmartRide.Common.Exceptions;
using SmartRide.Common.Extensions;
using SmartRide.Common.Responses;
using SmartRide.Common.Responses.Errors;
using SmartRide.Domain.Entities.Base;
using SmartRide.Domain.Interfaces;

namespace SmartRide.Application.Services;

public class AuthService(IMediator mediator, IPasswordHasher<User> passwordHasher, IJwtService jwtService) : IAuthService
{
    private readonly IMediator _mediator = mediator;
    private readonly IPasswordHasher<User> _passwordHasher = passwordHasher;
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
            var query = new GetUserByEmailQuery
            {
                Email = request.Email
            };
            var user = await _mediator.Send(query);

            if (user == null)
                return new ResponseDTO<AuthResponseDTO>
                {
                    Info = new ResponseInfo { Code = "USER_NOT_FOUND", Message = "User not found." }
                };

            var result = _passwordHasher.VerifyHashedPassword(null!, user.Password, request.Password);

            if (result != PasswordVerificationResult.Success)
                return new ResponseDTO<AuthResponseDTO>
                {
                    Info = new ResponseInfo { Code = "INVALID_PASSWORD", Message = "Invalid password." }
                };

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
            var command = MediatRFactory.CreateCommand<CreateUserCommand>(request);
            await _mediator.Send(command);
            await _mediator.Send(new SaveChangesCommand());

            return await LoginAsync(new LoginRequestDTO
            {
                Email = command.Email,
                Password = command.Password
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

    public ResponseDTO<ValidateTokenResponseDTO> ValidateToken(ValidateTokenRequestDTO request)
    {
        try
        {
            var payload = _jwtService.DecodeToken(request.AccessToken);
            var result = payload.ToObject<ValidateTokenResponseDTO>();

            return new ResponseDTO<ValidateTokenResponseDTO> { Data = result };
        }
        catch (BaseException ex)
        {
            return new ResponseDTO<ValidateTokenResponseDTO>
            {
                Info = ex.Info
            };
        }
        catch (Exception ex)
        {
            return new ResponseDTO<ValidateTokenResponseDTO>
            {
                Info = AuthErrors.VALIDATION_FAILED.FormatMessage(("Details", ex.Message))
            };
        }
    }
}
