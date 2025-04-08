using AutoMapper;
using System.Collections.Generic;
using System.Linq;

namespace SmartRide.Application.Mappings;

public class GenericMappingProfile : Profile
{
    public GenericMappingProfile()
    {
        // Map ICollection<T> to List<T>
        CreateMap(typeof(ICollection<>), typeof(List<>))
            .ConvertUsing(typeof(CollectionToListConverter<,>));
    }
}

// Generic converter for ICollection<T> to List<T>
public class CollectionToListConverter<TSource, TDestination> : ITypeConverter<ICollection<TSource>, List<TDestination>>
{
    public List<TDestination> Convert(ICollection<TSource> source, List<TDestination> destination, ResolutionContext context)
    {
        return source.Select(item => context.Mapper.Map<TDestination>(item)).ToList();
    }
}
