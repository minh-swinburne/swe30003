using AutoMapper;
using SmartRide.Application.Commands.Rides;
using SmartRide.Application.DTOs.Rides;
using SmartRide.Domain.Entities.Base;

namespace SmartRide.Application.Mappings;

public class RideMappingProfile : Profile
{
    public RideMappingProfile()
    {
        CreateMap<MatchRideCommand, Ride>();
        CreateMap<CreateRideCommand, Ride>()
            .ForMember(dest => dest.VehicleTypeId, opt => opt.MapFrom(src => src.VehicleType))
            .ForMember(dest => dest.VehicleType, opt => opt.Ignore());

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
        CreateMap<Ride, UpdateRideResponseDTO>();
        CreateMap<Ride, CreateRideResponseDTO>()
            .ForMember(dest => dest.RideStatus, opt => opt.MapFrom(src => src.Status))
            .ForMember(dest => dest.VehicleType, opt => opt.MapFrom(src => src.VehicleTypeId));
        CreateMap<Ride, MatchRideResponseDTO>()
            .ForMember(dest => dest.RideStatus, opt => opt.MapFrom(src => src.Status));
        CreateMap<Ride, GetRideResponseDTO>()
            .ForMember(dest => dest.RideStatus, opt => opt.MapFrom(src => src.Status));
        CreateMap<Ride, ListRideResponseDTO>()
            .ForMember(dest => dest.RideStatus, opt => opt.MapFrom(src => src.Status))
            .ForMember(dest => dest.VehicleType, opt => opt.MapFrom(src => src.VehicleTypeId)); ;
    }
}
