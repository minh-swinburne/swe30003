using AutoMapper;
using SmartRide.Application.DTOs.Vehicles;
using SmartRide.Domain.Entities.Base;

namespace SmartRide.Application.Mappings;

public class VehicleMappingProfile : Profile
{
    public VehicleMappingProfile()
    {
        CreateMap<Vehicle, GetVehicleByIdResponseDTO>()
            .ForMember(dest => dest.VehicleId, opt => opt.MapFrom(src => src.Id));
            // .ForMember(dest => dest.VehicleType, opt => opt.MapFrom(src => src.VehicleType));

        CreateMap<Vehicle, ListVehicleResponseDTO>()
            .ForMember(dest => dest.VehicleId, opt => opt.MapFrom(src => src.Id));
    }
}
