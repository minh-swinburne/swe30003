﻿using SmartRide.Application.DTOs.Lookup;
using SmartRide.Common.Interfaces;

namespace SmartRide.Application.DTOs.Users;

public class ListUserRequestDTO : BaseRequestDTO, ISortable, IPageable
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }

    public List<RoleDTO>? Roles { get; set; }
    public bool MatchAllRoles { get; set; } = false;

    public string? OrderBy { get; set; }
    public bool Ascending { get; set; }
    public int PageSize { get; set; } = 10;
    public int PageNo { get; set; } = 1;
}
