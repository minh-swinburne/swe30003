using AutoMapper;
using SmartRide.Application.Commands.Rides;
using SmartRide.Application.DTOs.Rides;
using SmartRide.Domain.Entities.Base;

namespace SmartRide.Application.Mappings;

public class RideMappingProfile : Profile
{
    public RideMappingProfile()
    {
        CreateMap<CreateRideCommand, Ride>();
        
        // Only map non-null properties
        CreateMap<UpdateRideCommand, Ride>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        // Common mapping for BaseRideResponseDTO
        CreateMap<Ride, BaseRideResponseDTO>()
            .ForMember(dest => dest.RideId, opt => opt.MapFrom(src => src.Id));

        // Derived DTOs inherit the mapping from BaseRideResponseDTO
        CreateMap<Ride, CreateRideResponseDTO>();
        CreateMap<Ride, UpdateRideResponseDTO>();
        CreateMap<Ride, GetRideResponseDTO>();
        CreateMap<Ride, ListRideResponseDTO>();
    }
}
