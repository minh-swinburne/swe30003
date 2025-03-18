using SmartRide.Common.Interfaces;
using SmartRide.Domain.Enums;

namespace SmartRide.Application.DTOs.Users;

public class ListUserRequestDTO : BaseDTO, ISortable, IPageable
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }

    public List<RoleEnum>? Roles { get; set; }
    public bool MatchAllRoles { get; set; } = false;

    public string? OrderBy { get; set; }
    public bool Ascending { get; set; }
    public int PageSize { get; set; } = 10;
    public int PageNo { get; set; } = 1;
}
