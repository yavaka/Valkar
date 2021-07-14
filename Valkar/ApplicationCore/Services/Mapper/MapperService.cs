namespace ApplicationCore.Services.Mapper
{
    using AutoMapper;
    
    public class MapperService : IMapperService
    {
        private readonly IMapper _mapper;

        public MapperService(IMapper mapper)
            => this._mapper = mapper;

        public TDestination Map<TSource, TDestination>(TSource source)
            => this._mapper.Map<TDestination>(source);
    }
}
