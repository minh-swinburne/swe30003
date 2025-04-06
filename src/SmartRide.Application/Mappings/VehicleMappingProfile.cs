using AutoMapper;
using SmartRide.Application.Commands.Vehicles;
using SmartRide.Application.DTOs.Vehicles;
using SmartRide.Domain.Entities.Base;

namespace SmartRide.Application.Mappings;

public class VehicleMappingProfile : Profile
{
    public VehicleMappingProfile()
    {
        CreateMap<CreateVehicleCommand, Vehicle>();

        // Only map non-null properties and ensure strings are not whitespace
        CreateMap<UpdateVehicleCommand, Vehicle>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) =>
                srcMember != null && (srcMember is not string str || !string.IsNullOrWhiteSpace(str))));

        // Common mapping for BaseVehicleResponseDTO
        CreateMap<Vehicle, BaseVehicleResponseDTO>()
            .ForMember(dest => dest.VehicleId, opt => opt.MapFrom(src => src.Id));

        // Derived DTOs inherit the mapping from BaseVehicleResponseDTO
        CreateMap<Vehicle, CreateVehicleResponseDTO>();
        CreateMap<Vehicle, UpdateVehicleResponseDTO>();
        CreateMap<Vehicle, GetVehicleResponseDTO>();
        CreateMap<Vehicle, ListVehicleResponseDTO>();
    }
}
