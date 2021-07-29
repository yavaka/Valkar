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
            CreateMap<DriverDetailsServiceModel, Driver>()
                .ForMember(d =>d.Documents, opt =>opt.Ignore());
            CreateMap<EmergencyContactServiceModel, EmergencyContact>();
            CreateMap<LimitedCompanyServiceModel, LimitedCompany>();
            CreateMap<Driver, SettingsServiceModel>()
                .ForMember(c => c.ChangePassword, opt => opt.Ignore());
            CreateMap<LimitedCompany, LimitedCompanyServiceModel>();
        }
    }
}
