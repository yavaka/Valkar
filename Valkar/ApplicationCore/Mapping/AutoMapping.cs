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
    using ApplicationCore.ServiceModels.Company;
    using System;
    using ApplicationCore.ServiceModels.Document;

    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            //Register
            CreateMap<RegisterServiceModel, User>();

            // Driver
            CreateMap<DriverDetailsServiceModel, Driver>()
                .ForMember(d => d.PersonalDocuments, opt => opt.Ignore());
            CreateMap<Driver, DriverDetailsServiceModel>();
            CreateMap<EmergencyContactServiceModel, EmergencyContact>();
            CreateMap<Driver, UpdateDriverDetailsServiceModel>();
            CreateMap<Driver, DriverProfileServiceModel>()
                .ForMember(d => d.DriverId, 
                    opt => opt.MapFrom(i => i.Id.ToString()))
                .ForMember(wd => wd.WorkedDays, opt => opt.Ignore());

            // Employee/Contractor LTD
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

            // Partner Comapny
            CreateMap<CompanyServiceModel, Company>()
                .ForMember(i => i.Id, 
                    opt => opt.MapFrom(i => Guid.Parse(i.Id)));
            CreateMap<Company, CompanyServiceModel>()
                .ForMember(i => i.Id,
                    opt => opt.MapFrom(i => i.Id.ToString()));

            // Sign PDF PersonalDocuments
            CreateMap<TempDocumentServiceModel, TempDocument>();
            CreateMap<TempDocument, TempDocumentServiceModel>()
                .ForMember(p => p.SentTo, opt => opt.Ignore());
        }
    }
}
