using AutoMapper;
using SmartRide.Application.DTOs.Users;
using SmartRide.Domain.Entities.Base;

namespace SmartRide.Application.Mappings;

public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        CreateMap<User, ListUserResponseDTO>()
            .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.Roles.Select(r => r.Id).ToList()));

        CreateMap<User, GetUserByIdResponseDTO>()
            .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.Roles.Select(r => r.Id).ToList()));
    }
}
