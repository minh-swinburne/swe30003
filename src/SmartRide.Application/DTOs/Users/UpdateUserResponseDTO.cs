namespace SmartRide.Application.DTOs.Users;

public class UpdateUserResponseDTO : BaseUserResponseDTO
{
    public required string FirstName { get; set; }
    public string? LastName { get; set; }
    public required string Email { get; set; }
    public required string Phone { get; set; }
    public string? Picture { get; set; }
}
