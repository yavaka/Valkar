﻿namespace ApplicationCore.Services.Admin
{
    using ApplicationCore.ServiceModels.Admin;
    using ApplicationCore.Services.Driver;
    using ApplicationCore.Services.Identity;
    using AutoMapper;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class AdminService : IAdminService
    {
        private readonly IDriverService _driverService;
        private readonly IIdentityService _identityService;
        private readonly IMapper _mapper;

        public AdminService(
            IDriverService driverService,
            IIdentityService identityService,
            IMapper mapper)
        {
            this._driverService = driverService;
            this._identityService = identityService;
            this._mapper = mapper;
        }

        public IEnumerable<DriverServiceModel> GetAllDrivers()
        {
            // Get all users including drivers 
            var users = this._identityService.GetAllUsers();
            
            var results = new List<DriverServiceModel>();
            foreach (var user in users)
            {
                //TODO: Enhance with AutoMapper
                results.Add(new DriverServiceModel 
                {
                    UserId = user.Id,
                    DriverId = user.Driver.Id.ToString(),
                    FirstNames = user.Driver.FirstNames,
                    Surname = user.Driver.Surname,
                    Email = user.Email,
                    PhoneNumber = user.Driver.PhoneNumber
                });
            }
            return results;
        }
    }
}