using Microsoft.AspNetCore.Mvc;
using SmartRide.API.Controllers.Attributes;
using SmartRide.API.Controllers.Base;
using SmartRide.Application.DTOs.Users;
using SmartRide.Application.Interfaces;

namespace SmartRide.API.Controllers.V1;

[Area("v1")]
[Pluralize]
public class UserController(IUserService userService) : BaseController
{
    private readonly IUserService _userService = userService;

    // GET: api/v1/user
    [HttpGet]
    public async Task<IActionResult> GetAllUsers([FromQuery] ListUserRequestDTO request)
    {
        var result = await _userService.GetAllUsersAsync(request);
        return Respond(result);
    }

    // GET api/v1/user/<userId>
    [HttpGet("{userId}")]
    public async Task<IActionResult> GetUserById([FromRoute] Guid userId)
    {
        var result = await _userService.GetUserByIdAsync(new GetUserByIdRequestDTO { UserId = userId });
        return Respond(result);
    }

    // POST api/v1/user
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CreateUserRequestDTO request)
    {
        var result = await _userService.CreateUserAsync(request);
        return Respond(result);
    }

    //// PUT api/v1/user/<userId>
    //[HttpPut("{id}")]
    //public void Put([FromRoute] int id, [FromBody] string value)
    //{
    //}

    //// DELETE api/v1/user/<userId>
    //[HttpDelete("{id}")]
    //public void Delete(int id)
    //{
    //}
}
