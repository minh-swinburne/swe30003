using AutoMapper;
using SmartRide.Application.DTOs.Lookup;
using SmartRide.Domain.Entities.Lookup;

namespace SmartRide.Application.Mappings;

public class LookupMappingProfile : Profile
{
    public LookupMappingProfile()
    {
        CreateMap<VehicleType, VehicleTypeDTO>()
            .ForMember(dest => dest.VehicleTypeId, opt => opt.MapFrom(src => src.Id));

        CreateMap<Role, RoleDTO>()
            .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.Id));
    }
}