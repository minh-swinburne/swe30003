using SmartRide.Application.DTOs.Users;

namespace SmartRide.Application.Queries.Users;

public class GetUserByPhoneQuery : BaseQuery<GetUserResponseDTO>
{
    public string Phone { get; init; } = string.Empty;
}
