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

        // Only map non-null properties and ensure strings are not whitespace
        CreateMap<UpdateUserCommand, User>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) =>
                srcMember != null && (srcMember is not string str || !string.IsNullOrWhiteSpace(str))));

        // Common mapping for BaseUserResponseDTO
        CreateMap<User, BaseUserResponseDTO>()
            .Include<User, CreateUserResponseDTO>()
            .Include<User, UpdateUserResponseDTO>()
            .Include<User, GetUserResponseDTO>()
            .Include<User, ListUserResponseDTO>()
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id));

        // Derived DTOs inherit the mapping from BaseUserResponseDTO
        CreateMap<User, CreateUserResponseDTO>();
        CreateMap<User, UpdateUserResponseDTO>();
        CreateMap<User, GetUserResponseDTO>().ReverseMap();
        CreateMap<User, ListUserResponseDTO>()
            .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.Roles));
    }
}
