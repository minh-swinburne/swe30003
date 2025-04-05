﻿using AutoMapper;
using SmartRide.Application.Commands.Users;
using SmartRide.Application.DTOs.Users;
using SmartRide.Domain.Entities.Base;

namespace SmartRide.Application.Mappings;

public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        CreateMap<CreateUserCommand, User>();

        CreateMap<User, CreateUserResponseDTO>()
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id));

        CreateMap<User, GetUserByIdResponseDTO>()
            .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.Roles.Select(r => r.Id).ToList()))
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id));

        CreateMap<User, ListUserResponseDTO>()
            .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.Roles.Select(r => r.Id).ToList()))
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id));
    }
}
