using Microsoft.AspNetCore.Mvc;
using SmartRide.API.Controllers.Attributes;
using SmartRide.API.Controllers.Base;
using SmartRide.Application.DTOs.Locations;
using SmartRide.Application.Interfaces;

namespace SmartRide.API.Controllers.V1;

[Area("v1")]
[Pluralize]
public class LocationController(ILocationService locationService) : BaseController
{
    private readonly ILocationService _locationService = locationService;

    // GET: api/v1/location
    [HttpGet]
    public async Task<IActionResult> GetAllLocations([FromQuery] ListLocationRequestDTO request)
    {
        var result = await _locationService.ListLocationsAsync(request);
        return Respond(result);
    }

    // GET api/v1/location/<locationId>
    [HttpGet("{locationId}")]
    public async Task<IActionResult> GetLocationById([FromRoute] Guid locationId)
    {
        var result = await _locationService.GetLocationByIdAsync(new GetLocationByIdRequestDTO { LocationId = locationId });
        return Respond(result);
    }

    // POST api/v1/location
    [HttpPost]
    public async Task<IActionResult> CreateLocation([FromBody] CreateLocationRequestDTO request)
    {
        var result = await _locationService.CreateLocationAsync(request);
        return Respond(result);
    }

    // PUT api/v1/location/<locationId>
    [HttpPut("{locationId}")]
    public async Task<IActionResult> UpdateLocation([FromRoute] Guid locationId, [FromBody] UpdateLocationRequestDTO request)
    {
        request.LocationId = locationId;
        var result = await _locationService.UpdateLocationAsync(request);
        return Respond(result);
    }

    // DELETE api/v1/location/<locationId>
    [HttpDelete("{locationId}")]
    public async Task<IActionResult> DeleteLocation([FromRoute] Guid locationId)
    {
        var result = await _locationService.DeleteLocationAsync(new DeleteLocationRequestDTO { LocationId = locationId });
        return Respond(result);
    }
}
