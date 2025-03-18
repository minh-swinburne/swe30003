using AutoMapper;
using SmartRide.Application.DTOs;

namespace SmartRide.Application.Mappings;

public class GenericMappingProfile : Profile
{
    public GenericMappingProfile()
    {
        // Generic mapping from List<T> to ListResponseDTO<T>
        CreateMap(typeof(List<>), typeof(ListResponseDTO<>))
            .ForMember("Data", opt => opt.MapFrom(src => src))
            .ForMember("Count", opt => opt.MapFrom(src => ((IList<object>)src).Count))
            .ForMember("Metadata", opt => opt.MapFrom(_ => new Dictionary<string, object> { { "timestamp", DateTime.UtcNow } }));
    }
}
