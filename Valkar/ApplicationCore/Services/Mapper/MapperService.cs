namespace ApplicationCore.Services.Mapper
{
    using AutoMapper;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    public class MapperService : IMapperService
    {
        private readonly IMapper _mapper;

        public MapperService(IMapper mapper)
            => this._mapper = mapper;

        public TDestination Map<TSource, TDestination>(TSource source)
            => this._mapper.Map<TDestination>(source);

        public ICollection<TDestination> Map<TSource, TDestination>(ICollection<TSource> sources) 
            => this._mapper.Map< ICollection<TSource>, ICollection<TDestination>>(sources);
    }
}
