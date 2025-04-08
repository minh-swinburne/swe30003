using System.Text.Json.Serialization;
using SmartRide.Application.DTOs.Lookup;

namespace SmartRide.Application.DTOs.Users;

public class GetUserResponseDTO : BaseUserResponseDTO
{
    public required string FirstName { get; set; }
    public string? LastName { get; set; }
    public required string Email { get; set; }
    public required string Phone { get; set; }
    public string? Picture { get; set; }
    public required List<RoleDTO> Roles { get; set; }

    [JsonIgnore] // Exclude Password from serialization
    public string? Password { get; set; }
}
