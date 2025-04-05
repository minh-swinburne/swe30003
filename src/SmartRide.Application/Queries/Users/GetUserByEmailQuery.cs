using SmartRide.Application.DTOs.Users;

namespace SmartRide.Application.Queries.Users;

public class GetUserByEmailQuery : BaseQuery<GetUserResponseDTO>
{
    public string Email { get; init; } = string.Empty;
}
