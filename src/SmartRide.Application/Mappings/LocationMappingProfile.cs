using AutoMapper;
using SmartRide.Application.Commands.Locations;
using SmartRide.Application.DTOs.Locations;
using SmartRide.Domain.Entities.Base;

namespace SmartRide.Application.Mappings;

public class LocationMappingProfile : Profile
{
    public LocationMappingProfile()
    {
        CreateMap<CreateLocationCommand, Location>();

        // Only map non-null properties and ensure strings are not whitespace
        CreateMap<UpdateLocationCommand, Location>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) =>
                srcMember != null &&
                (srcMember is not string str || !string.IsNullOrWhiteSpace(str)) &&
                (!srcMember.GetType().IsValueType || Nullable.GetUnderlyingType(srcMember.GetType()) != null)
            ));

        // Common mapping for BaseLocationResponseDTO
        CreateMap<Location, BaseLocationResponseDTO>()
            .Include<Location, CreateLocationResponseDTO>()
            .Include<Location, UpdateLocationResponseDTO>()
            .Include<Location, GetLocationResponseDTO>()
            .Include<Location, ListLocationResponseDTO>()
            .ForMember(dest => dest.LocationId, opt => opt.MapFrom(src => src.Id));

        // Derived DTOs inherit the mapping from BaseLocationResponseDTO
        CreateMap<Location, CreateLocationResponseDTO>();
        CreateMap<Location, UpdateLocationResponseDTO>();
        CreateMap<Location, GetLocationResponseDTO>();
        CreateMap<Location, ListLocationResponseDTO>();

        // Additional mappings
        CreateMap<ListLocationResponseDTO, GetLocationResponseDTO>();
        CreateMap<CreateLocationResponseDTO, GetLocationResponseDTO>();
    }
}
