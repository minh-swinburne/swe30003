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
        CreateMap<MatchRideCommand, Ride>();

        // Only map non-null properties and ensure strings are not whitespace
        CreateMap<UpdateRideCommand, Ride>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) =>
                srcMember != null && (srcMember is not string str || !string.IsNullOrWhiteSpace(str))));

        // Common mapping for BaseRideResponseDTO
        CreateMap<Ride, BaseRideResponseDTO>()
            .Include<Ride, CreateRideResponseDTO>()
            .Include<Ride, UpdateRideResponseDTO>()
            .Include<Ride, MatchRideResponseDTO>()
            .Include<Ride, GetRideResponseDTO>()
            .Include<Ride, ListRideResponseDTO>()
            .ForMember(dest => dest.RideId, opt => opt.MapFrom(src => src.Id));

        // Derived DTOs inherit the mapping from BaseRideResponseDTO
        CreateMap<Ride, CreateRideResponseDTO>();
        CreateMap<Ride, UpdateRideResponseDTO>();
        CreateMap<Ride, MatchRideResponseDTO>();
        CreateMap<Ride, GetRideResponseDTO>();
        CreateMap<Ride, ListRideResponseDTO>();
    }
}
