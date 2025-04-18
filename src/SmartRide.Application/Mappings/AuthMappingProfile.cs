using AutoMapper;
using SmartRide.Application.DTOs.Auth;
using SmartRide.Application.DTOs.Users;

namespace SmartRide.Application.Mappings;

public class AuthMappingProfile : Profile
{
    public AuthMappingProfile()
    {
        CreateMap<GetCurrentUserRequestDTO, ValidateTokenRequestDTO>()
            .ReverseMap();
    }
}
