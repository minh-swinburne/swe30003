﻿using SmartRide.Application.DTOs.Lookup;

namespace SmartRide.Application.DTOs.Users;

public class GetUserByIdResponseDTO : BaseUserResponseDTO
{
    public required string FirstName { get; set; }
    public string? LastName { get; set; }
    public required string Email { get; set; }
    public required string Phone { get; set; }
    public string? Picture { get; set; }
    public required List<RoleDTO> Roles { get; set; }
    //public string? Address { get; set; }  // From identity - implement later
}
