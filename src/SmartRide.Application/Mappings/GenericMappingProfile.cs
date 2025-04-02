using AutoMapper;

namespace SmartRide.Application.Mappings;

public class GenericMappingProfile : Profile
{
    public GenericMappingProfile()
    {
        //CreateMap(typeof(List<>), typeof(ListResponseDTO<>))
        //    .ForMember("Data", opt => opt.MapFrom(src => src))
        //    .ForMember("Count", opt => opt.MapFrom((src, dest, _, context) =>
        //    {
        //        return src is IEnumerable enumerable ? enumerable.Cast<object>().Count() : 0;
        //    }))
        //    .ForMember("Metadata", opt => opt.MapFrom(_ => new Dictionary<string, object>
        //    {
        //        { "timestamp", DateTime.UtcNow }
        //    }));
    }
}
