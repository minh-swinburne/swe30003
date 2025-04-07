using Microsoft.AspNetCore.Mvc;
using SmartRide.API.Controllers.Attributes;
using SmartRide.API.Controllers.Base;
using SmartRide.Application.DTOs.Rides;
using SmartRide.Application.Interfaces;

namespace SmartRide.API.Controllers.V1;

[Area("v1")]
[Pluralize]
public class RideController(IRideService rideService) : BaseController
{
    private readonly IRideService _rideService = rideService;

    // GET: api/v1/ride
    [HttpGet]
    public async Task<IActionResult> GetAllRides([FromQuery] ListRideRequestDTO request)
    {
        var result = await _rideService.ListRidesAsync(request);
        return Respond(result);
    }

    // GET api/v1/ride/<rideId>
    [HttpGet("{rideId}")]
    public async Task<IActionResult> GetRideById([FromRoute] Guid rideId)
    {
        var result = await _rideService.GetRideByIdAsync(new GetRideByIdRequestDTO { RideId = rideId });
        return Respond(result);
    }

    // POST api/v1/ride
    [HttpPost]
    public async Task<IActionResult> CreateRide([FromBody] CreateRideRequestDTO request)
    {
        var result = await _rideService.CreateRideAsync(request);
        return Respond(result);
    }

    // PUT api/v1/ride/<rideId>
    [HttpPut("{rideId}")]
    public async Task<IActionResult> UpdateRide([FromRoute] Guid rideId, [FromBody] UpdateRideRequestDTO request)
    {
        request.RideId = rideId;
        var result = await _rideService.UpdateRideAsync(request);
        return Respond(result);
    }

    // DELETE api/v1/ride/<rideId>
    [HttpDelete("{rideId}")]
    public async Task<IActionResult> DeleteRide([FromRoute] Guid rideId)
    {
        var result = await _rideService.DeleteRideAsync(new DeleteRideRequestDTO { RideId = rideId });
        return Respond(result);
    }
}
