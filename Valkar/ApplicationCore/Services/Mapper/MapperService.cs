namespace ApplicationCore.Services.Mapper
{
    using AutoMapper;
    using System.Collections.Generic;

    public class MapperService : IMapperService
    {
        private readonly IMapper _mapper;

        public MapperService(IMapper mapper)
            => this._mapper = mapper;

        public TDestination Map<TSource, TDestination>(TSource source)
            => this._mapper.Map<TDestination>(source);

        public IEnumerable<TDestination> Map<TSource, TDestination>(TSource[] sources) 
            => this._mapper.Map<TSource[], IEnumerable<TDestination>>(sources);
    }
}
