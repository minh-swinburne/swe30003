using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SmartRide.API.Controllers.Attributes;
using SmartRide.Application.DTOs.Users;
using SmartRide.Application.Interfaces;

namespace SmartRide.API.Controllers.V1;

[Area("v1")]
[Pluralize]
[Authorize] // Require authentication for all actions in this controller
public class UserController(IUserService userService) : BaseController
{
    private readonly IUserService _userService = userService;

    // GET: api/v1/user
    [HttpGet]
    public async Task<IActionResult> GetAllUsers([FromQuery] ListUserRequestDTO request)
    {
        var result = await _userService.ListUsersAsync(request);
        return Respond(result);
    }

    // GET api/v1/user/<userId>
    [HttpGet("{userId}")]
    public async Task<IActionResult> GetUserById([FromRoute] Guid userId)
    {
        var result = await _userService.GetUserByIdAsync(new GetUserByIdRequestDTO { UserId = userId });
        return Respond(result);
    }

    // GET api/v1/user/email/<email>
    [HttpGet("email/{email}")]
    public async Task<IActionResult> GetUserByEmail([FromRoute] string email)
    {
        var result = await _userService.GetUserByEmailAsync(new GetUserByEmailRequestDTO { Email = email });
        return Respond(result);
    }

    // GET api/v1/user/phone/<phone>
    [HttpGet("phone/{phone}")]
    public async Task<IActionResult> GetUserByPhone([FromRoute] string phone)
    {
        var result = await _userService.GetUserByPhoneAsync(new GetUserByPhoneRequestDTO { Phone = phone });
        return Respond(result);
    }

    // POST api/v1/user
    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserRequestDTO request)
    {
        var result = await _userService.CreateUserAsync(request);
        return Respond(result);
    }

    // PUT api/v1/user/<userId>
    [HttpPut("{userId}")]
    public async Task<IActionResult> UpdateUser([FromRoute] Guid userId, [FromBody] UpdateUserRequestDTO request)
    {
        request.UserId = userId;
        var result = await _userService.UpdateUserAsync(request);
        return Respond(result);
    }

    // DELETE api/v1/user/<userId>
    [HttpDelete("{userId}")]
    public async Task<IActionResult> DeleteUser([FromRoute] Guid userId)
    {
        var result = await _userService.DeleteUserAsync(new DeleteUserRequestDTO { UserId = userId });
        return Respond(result);
    }
}
