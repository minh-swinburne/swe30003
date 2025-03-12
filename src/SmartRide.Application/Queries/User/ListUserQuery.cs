using MediatR;
using SmartRide.Application.DTOs;
using SmartRide.Application.DTOs.Users;
using SmartRide.Common.Interfaces;

namespace SmartRide.Application.Queries.Users;

public class ListUserQuery : IRequest<ListResponseDTO<ListUserResponseDTO>>, IPageable, ISortable
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }

    public int PageSize { get; set; }
    public int PageNo { get; set; }
    public string? OrderBy { get; set; }
    public bool Ascending { get; set; }
}
