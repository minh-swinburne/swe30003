using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartRide.Application.DTOs.Auth;
using SmartRide.Application.DTOs.Users;
using SmartRide.Application.Interfaces;
using SmartRide.Domain.Interfaces;
using System.Security.Claims;

namespace SmartRide.API.Controllers.V1;

[Area("v1")]
[AllowAnonymous]
public class AuthController(IUserService userService, IJwtService jwtService) : BaseController
{
    private readonly IUserService _userService = userService;
    private readonly IJwtService _jwtService = jwtService;

    // POST: api/v1/auth/login
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDTO request)
    {
        var userResponse = await _userService.GetUserByEmailAsync(new GetUserByEmailRequestDTO { Email = request.Email });

        if (userResponse.Data == null || userResponse.Data.Password != request.Password)
        {
            return Unauthorized(new { Message = "Invalid email or password." });
        }

        var user = userResponse.Data;
        var roles = user.Roles.Select(r => r.Name).ToList();

        var token = _jwtService.GenerateToken(
            [
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.FirstName),
                new Claim(ClaimTypes.Role, string.Join(",", roles))
            ],
            expiration: TimeSpan.FromMinutes(60)
        );

        return Ok(new { AccessToken = token });
    }

    // POST: api/v1/auth/register
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] CreateUserRequestDTO request)
    {
        var userResponse = await _userService.CreateUserAsync(request);
        return CreatedAtAction(nameof(Login), new { email = userResponse.Data.Email }, userResponse.Data);
    }

    // POST: api/v1/auth/validate
    [HttpPost("validate")]
    public IActionResult ValidateToken([FromBody] ValidateTokenRequestDTO request)
    {
        var principal = _jwtService.ValidateToken(request.Token);

        if (principal == null)
        {
            return Unauthorized(new { Message = "Invalid or expired token." });
        }

        var claims = principal.Claims.Select(c => new { c.Type, c.Value }).ToList();
        return Ok(new { Valid = true, Claims = claims });
    }
}
