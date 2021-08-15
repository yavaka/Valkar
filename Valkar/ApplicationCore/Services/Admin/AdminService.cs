namespace ApplicationCore.Services.Admin
{
    using ApplicationCore.ServiceModels.Admin;
    using ApplicationCore.ServiceModels.Driver;
    using ApplicationCore.Services.Driver;
    using ApplicationCore.Services.Identity;
    using AutoMapper;
    using Infrastructure.Models;
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

        public IEnumerable<DriverAdminServiceModel> GetAllDrivers()
        {
            // Get all users including drivers 
            var users = this._identityService.GetAllUsers();

            var results = new List<DriverAdminServiceModel>();
            foreach (var user in users)
            {
                //TODO: Enhance with AutoMapper
                results.Add(new DriverAdminServiceModel
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

        public DriverAdminServiceModel GetDriverProfile(string userId)
        {
            // Get the user including the driver entity
            var user = this._identityService.GetUserById(userId);
            var driver = new DriverAdminServiceModel
            {
                UserId = user.Id,
                Email = user.Email,
                DriverId = user.Driver.Id.ToString(),
                FirstNames = user.Driver.FirstNames,
                Surname = user.Driver.Surname,
                PhoneNumber = user.Driver.PhoneNumber,
                Address = user.Driver.Address,
                Postcode = user.Driver.Postcode,
                DateOfBirth = user.Driver.DateOfBirth,
                CreatedOn = user.RegisteredOn,
                NiNo = user.Driver.NationalInsuranceNumber,
                LimitedCompany = user.Driver.LimitedCompany.CompanyName,
                CompanyRegistrationNumber = user.Driver.LimitedCompany.CompanyRegistrationNumber,
                EmergencyContact = this._mapper.Map<EmergencyContact, EmergencyContactServiceModel>(user.Driver.EmergencyContact)
            };
            driver.ConvertDrivingLicenceEntitiesToCategoriesName(user.Driver.LicenceCategories);
            return driver;
        }
    }
}
