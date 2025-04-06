namespace SmartRide.Application.DTOs;

public abstract class BaseRequestDTO
{
    public DateTime? CreatedTimeFrom { get; set; }
    public DateTime? CreatedTimeTo { get; set; }
    public DateTime? UpdatedTimeFrom { get; set; }
    public DateTime? UpdatedTimeTo { get; set; }
}
