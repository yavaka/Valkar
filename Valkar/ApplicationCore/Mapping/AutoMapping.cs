namespace ApplicationCore.Mapping
{
    using AutoMapper;
    using Infrastructure.Models;
    using ApplicationCore.ServiceModels.Identity;
    using ApplicationCore.ServiceModels.Driver;
    using ApplicationCore.ServiceModels.Admin;
    using System.Collections.Generic;

    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<RegisterServiceModel, User>();
            CreateMap<DriverDetailsServiceModel, Driver>()
                .ForMember(d => d.Documents, opt => opt.Ignore());
            CreateMap<EmergencyContactServiceModel, EmergencyContact>();
            CreateMap<LimitedCompanyServiceModel, LimitedCompany>();
            CreateMap<Driver, UpdateDriverDetailsServiceModel>();
            CreateMap<LimitedCompany, LimitedCompanyServiceModel>();
            CreateMap<EmergencyContact, EmergencyContactServiceModel>();
            // Collections
            CreateMap<Driver[], IEnumerable<DriverAdminServiceModel>>();
        }
    }
}
