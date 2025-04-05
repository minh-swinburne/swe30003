namespace SmartRide.Application.DTOs.Users;

public class UpdateUserRequestDTO : BaseRequestDTO
{
    public required Guid UserId { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Password { get; set; }
    public string? Picture { get; set; }
}
