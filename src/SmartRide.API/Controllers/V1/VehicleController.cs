using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SmartRide.API.Controllers.Attributes;
using SmartRide.Application.DTOs.Vehicles;
using SmartRide.Application.Interfaces;

namespace SmartRide.API.Controllers.V1;

[Area("v1")]
[Pluralize]
[Authorize] // Require authentication for all actions in this controller
public class VehicleController(IVehicleService vehicleService) : BaseController
{
    private readonly IVehicleService _vehicleService = vehicleService;

    // GET: api/v1/vehicle
    [HttpGet]
    public async Task<IActionResult> GetAllVehicles([FromQuery] ListVehicleRequestDTO request)
    {
        var result = await _vehicleService.ListVehiclesAsync(request);
        return Respond(result);
    }

    // GET api/v1/vehicle/<vehicleId>
    [HttpGet("{vehicleId}")]
    public async Task<IActionResult> GetVehicleById([FromRoute] Guid vehicleId)
    {
        var result = await _vehicleService.GetVehicleByIdAsync(new GetVehicleByIdRequestDTO { VehicleId = vehicleId });
        return Respond(result);
    }

    // GET api/v1/vehicle/vin/<vin>
    [HttpGet("vin/{vin}")]
    public async Task<IActionResult> GetVehicleByVin([FromRoute] string vin)
    {
        var result = await _vehicleService.GetVehicleByVinAsync(new GetVehicleByVinRequestDTO { Vin = vin });
        return Respond(result);
    }

    // GET api/v1/vehicle/plate/<plate>
    [HttpGet("plate/{plate}")]
    public async Task<IActionResult> GetVehicleByPlate([FromRoute] string plate)
    {
        var result = await _vehicleService.GetVehicleByPlateAsync(new GetVehicleByPlateRequestDTO { Plate = plate });
        return Respond(result);
    }

    // POST api/v1/vehicle
    [HttpPost]
    public async Task<IActionResult> CreateVehicle([FromBody] CreateVehicleRequestDTO request)
    {
        var result = await _vehicleService.CreateVehicleAsync(request);
        return Respond(result);
    }

    // PUT api/v1/vehicle/<vehicleId>
    [HttpPut("{vehicleId}")]
    public async Task<IActionResult> UpdateVehicle([FromRoute] Guid vehicleId, [FromBody] UpdateVehicleRequestDTO request)
    {
        request.VehicleId = vehicleId;
        var result = await _vehicleService.UpdateVehicleAsync(request);
        return Respond(result);
    }

    // DELETE api/v1/vehicle/<vehicleId>
    [HttpDelete("{vehicleId}")]
    public async Task<IActionResult> DeleteVehicle([FromRoute] Guid vehicleId)
    {
        var result = await _vehicleService.DeleteVehicleAsync(new DeleteVehicleRequestDTO { VehicleId = vehicleId });
        return Respond(result);
    }
}
