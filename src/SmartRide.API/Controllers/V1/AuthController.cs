using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartRide.Application.DTOs.Auth;
using SmartRide.Application.DTOs.Users;
using SmartRide.Application.Interfaces;

namespace SmartRide.API.Controllers.V1;

[Area("v1")]
[AllowAnonymous]
public class AuthController(IAuthService authService) : BaseController
{
    private readonly IAuthService _authService = authService;

    // POST: api/v1/auth/login
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDTO request)
    {
        var result = await _authService.LoginAsync(request);
        return Respond(result);
    }

    // POST: api/v1/auth/register
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] CreateUserRequestDTO request)
    {
        var result = await _authService.RegisterAsync(request);
        return Respond(result, StatusCodes.Status201Created);
    }

    // POST: api/v1/auth/validate
    [HttpPost("validate")]
    public IActionResult ValidateToken([FromBody] ValidateTokenRequestDTO request)
    {
        var result = _authService.ValidateToken(request);
        return Respond(result);
    }
}
