namespace ApplicationCore.Mapping
{
    using AutoMapper;
    using Infrastructure.Models;
    using ApplicationCore.ServiceModels.Identity;
    using ApplicationCore.ServiceModels.Driver;
    using ApplicationCore.ServiceModels.Admin;
    using System.Collections.Generic;
    using ApplicationCore.ServiceModels.WorkingDay;
    using System.Linq;

    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            //Register
            CreateMap<RegisterServiceModel, User>();

            // Driver
            CreateMap<DriverDetailsServiceModel, Driver>()
                .ForMember(d => d.Documents, opt => opt.Ignore());
            CreateMap<EmergencyContactServiceModel, EmergencyContact>();
            CreateMap<Driver, UpdateDriverDetailsServiceModel>();
            CreateMap<Driver, DriverProfileServiceModel>()
                .ForMember(d => d.DriverId, 
                    opt => opt.MapFrom(i => i.Id.ToString()))
                .ForMember(wd => wd.WorkedDays, opt => opt.Ignore());
            
            // LTD
            CreateMap<LimitedCompanyServiceModel, LimitedCompany>();
            CreateMap<LimitedCompany, LimitedCompanyServiceModel>();
            
            // Emergency contact
            CreateMap<EmergencyContact, EmergencyContactServiceModel>();

            // Working Day
            CreateMap<WorkingDayServiceModel, WorkingDay>();
            CreateMap<WorkingDay, WorkingDayServiceModel>();

            // Collections
            CreateMap<Driver[], IEnumerable<DriverAdminServiceModel>>();
            CreateMap<WorkingDay[], IEnumerable<WorkingDayServiceModel>>();
        }
    }
}
