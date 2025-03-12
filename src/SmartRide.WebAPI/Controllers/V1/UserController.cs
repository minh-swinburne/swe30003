using Microsoft.AspNetCore.Mvc;
using SmartRide.Application.DTOs.Users;
using SmartRide.Application.Interfaces;
using SmartRide.WebAPI.Controllers.Attributes;
using SmartRide.WebAPI.Controllers.Base;

namespace SmartRide.WebAPI.Controllers.V1;

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
    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserById([FromRoute] int id)
    {
        var result = await _userService.GetUserByIdAsync(new GetUserRequestDTO { Id = id });
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
