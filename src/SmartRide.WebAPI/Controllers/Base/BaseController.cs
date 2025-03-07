using Microsoft.AspNetCore.Mvc;
using SmartRide.Application.DTOs;
using SmartRide.WebAPI.Controllers.Attributes;

namespace SmartRide.WebAPI.Controllers.Base;

[Route("api/[area]/[controller]")]
[ApiController]
public class BaseController : ControllerBase
{
    internal IActionResult Respond<T>(
        ResponseDTO<T> response,
        int statusCode = StatusCodes.Status200OK
        )
    {
        return StatusCode(statusCode, response);
    }

    internal IActionResult Respond<T>(
        ListResponseDTO<T> response,
        int statusCode = StatusCodes.Status200OK
        )
    {
        return StatusCode(statusCode, response);
    }
}
