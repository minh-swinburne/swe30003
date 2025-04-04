using SmartRide.Application.DTOs.Users;

namespace SmartRide.Application.Queries.Users;

public class GetUserByIdQuery : BaseQuery<GetUserByIdResponseDTO>
{
    public Guid UserId { get; init; }
}
