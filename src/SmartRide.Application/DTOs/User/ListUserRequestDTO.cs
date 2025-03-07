using SmartRide.Common.Interfaces;

namespace SmartRide.Application.DTOs.User;

public class ListUserRequestDTO : ISortable, IPageable
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }

    public string? OrderBy { get; set; }
    public bool Ascending { get; set; }
    public int PageSize { get; set; } = 10;
    public int PageNo { get; set; } = 1;
}
