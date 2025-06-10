using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartRide.Application.DTOs;
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

        if (result.Data?.AccessToken != null)
        {
            // Set JWT as HttpOnly, Secure cookie
            Response.Cookies.Append("access_token", result.Data.AccessToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true, // Only over HTTPS in production!
                SameSite = SameSiteMode.Lax, // Or Lax, depending on your needs
                Expires = DateTimeOffset.UtcNow.AddHours(1)
            });
        }

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
    public IActionResult ValidateToken([FromHeader(Name = "Authorization")] string token)
    {
        if (!token.StartsWith("Bearer "))
            throw new ArgumentException("Invalid token.");

        var request = new ValidateTokenRequestDTO { AccessToken = token.Split(" ")[1] };
        var result = _authService.ValidateToken(request);
        return Respond(result);
    }

    // POST: api/v1/auth/logout
    [HttpPost("logout")]
    public IActionResult Logout()
    {
        Response.Cookies.Delete("access_token");
        var result = new ResponseDTO<bool> { Data = true };
        return Respond(result);
    }
}
