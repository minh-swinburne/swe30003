using SmartRide.Application.DTOs.Users;
using SmartRide.Common.Interfaces;
using SmartRide.Domain.Enums;

namespace SmartRide.Application.Queries.Users;

public class ListUserQuery : BaseQuery<List<ListUserResponseDTO>>, IPageable, ISortable
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }

    public List<RoleEnum>? Roles { get; set; }
    public bool MatchAllRoles { get; set; } = false;

    public string? OrderBy { get; set; }
    public bool Ascending { get; set; } = true;
    public int PageSize { get; set; } = 10;
    public int PageNo { get; set; } = 1;
}
