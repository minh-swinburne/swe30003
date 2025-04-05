using AutoMapper;
using SmartRide.Application.Commands.Users;
using SmartRide.Application.DTOs.Users;
using SmartRide.Domain.Entities.Base;

namespace SmartRide.Application.Mappings;

public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        CreateMap<CreateUserCommand, User>();

        // Common mapping for BaseUserResponseDTO
        CreateMap<User, BaseUserResponseDTO>()
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id));

        // Derived DTOs inherit the mapping from BaseUserResponseDTO
        CreateMap<User, CreateUserResponseDTO>();
        CreateMap<User, UpdateUserResponseDTO>();
        CreateMap<User, GetUserByIdResponseDTO>();
        CreateMap<User, ListUserResponseDTO>();
    }
}
