using Microsoft.AspNetCore.Mvc;
using SmartRide.Application.DTOs;

namespace SmartRide.WebAPI.Controllers.Base;

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
