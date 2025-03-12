using AutoMapper;
using SmartRide.Application.DTOs.Users;
using SmartRide.Domain.Entities;

namespace SmartRide.Application.Mappings;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, ListUserResponseDTO>().ReverseMap();
    }
}
