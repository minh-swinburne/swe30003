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
    }
}
