namespace SmartRide.Application.DTOs.Auth;

public class ValidateTokenResponseDTO
{
    public Dictionary<string, object> Header { get; set; } = [];
    public Dictionary<string, object> Payload { get; set; } = [];
}
