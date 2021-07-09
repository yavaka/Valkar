namespace ApplicationCore.Mapping
{
    using AutoMapper;
    using Infrastructure.Models;
    using ApplicationCore.ServiceModels.Identity;

    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<RegisterServiceModel, User>();
        }
    }
}
