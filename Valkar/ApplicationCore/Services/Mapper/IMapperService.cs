using System.Collections.Generic;

namespace ApplicationCore.Services.Mapper
{
    public interface IMapperService
    {
        TDestination Map<TSource, TDestination>(TSource source);

        IEnumerable<TDestination> Map<TSource, TDestination>(TSource[] sources);
    }
}
