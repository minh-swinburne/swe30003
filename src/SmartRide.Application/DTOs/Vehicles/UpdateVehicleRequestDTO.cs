namespace SmartRide.Application.DTOs.Vehicles;

public class UpdateVehicleRequestDTO : BaseRequestDTO
{
    public Guid VehicleId { get; set; }
    public string? Plate { get; set; }
    public string? Make { get; set; }
    public string? Model { get; set; }
    public int? Year { get; set; }
    public DateTime? RegisteredDate { get; set; }
}
