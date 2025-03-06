using Microsoft.AspNetCore.Mvc;
using SmartRide.Application.DTOs;
using SmartRide.Common.Responses;

namespace SmartRide.WebAPI.Controllers.Base;

public class BaseController : ControllerBase
{
    internal IActionResult Respond<T>(
        T data,
        int statusCode = StatusCodes.Status200OK, 
        ResponseInfo? info = null,
        List<ResponseInfo>? warnings = null,
        Dictionary<string, object>? metadata = null
        )
    {
        ResponseDTO<T> response = new()
        {
            Data = data,
            Code = info?.Code,
            Module = info?.Module,
            Message = info?.Message,
            Warnings = warnings,
            Metadata = metadata ?? new Dictionary<string, object> { { "timestamp", DateTime.UtcNow } },
        };

        return StatusCode(statusCode, response);
    }

    internal IActionResult Respond<T>(
        IEnumerable<T> data,
        int count,
        int statusCode = StatusCodes.Status200OK,
        ResponseInfo? info = null,
        List<ResponseInfo>? warnings = null,
        Dictionary<string, object>? metadata = null
        )
    {
        ListResponseDTO<T> response = new()
        {
            Data = data,
            Count = count,
            Code = info?.Code,
            Module = info?.Module,
            Message = info?.Message,
            Warnings = warnings,
            Metadata = metadata ?? new Dictionary<string, object> { { "timestamp", DateTime.UtcNow } },
        };
        return StatusCode(statusCode, response);
    }
}
