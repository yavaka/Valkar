using System.Collections.Generic;

namespace ApplicationCore.Services.Mapper
{
    public interface IMapperService
    {
        TDestination Map<TSource, TDestination>(TSource source);

        ICollection<TDestination> Map<TSource, TDestination>(ICollection<TSource> sources);
    }
}
