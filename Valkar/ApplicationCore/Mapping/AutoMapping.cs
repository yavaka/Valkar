namespace ApplicationCore.Mapping
{
    using AutoMapper;
    using Infrastructure.Models;
    using ApplicationCore.ServiceModels.Identity;
    using ApplicationCore.ServiceModels.Driver;

    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<RegisterServiceModel, User>();
            CreateMap<DriverDetailsServiceModel, Driver>();
            CreateMap<EmergencyContactServiceModel, EmergencyContact>();
            CreateMap<LimitedCompanyServiceModel, LimitedCompany>();
        }
    }
}
