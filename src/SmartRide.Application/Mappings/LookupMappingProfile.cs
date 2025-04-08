using AutoMapper;
using SmartRide.Application.DTOs.Lookup;
using SmartRide.Domain.Entities.Lookup;

namespace SmartRide.Application.Mappings;

public class LookupMappingProfile : Profile
{
    public LookupMappingProfile()
    {
        CreateMap<Role, RoleDTO>()
            .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.Id))
            .ReverseMap();

        CreateMap<VehicleType, VehicleTypeDTO>()
            .ForMember(dest => dest.VehicleTypeId, opt => opt.MapFrom(src => src.Id));

        CreateMap<PaymentMethod, PaymentMethodDTO>()
            .ForMember(dest => dest.PaymentMethodId, opt => opt.MapFrom(src => src.Id));
    }
}