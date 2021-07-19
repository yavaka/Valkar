namespace ApplicationCore.Services.Driver
{
    using ApplicationCore.Helpers;
    using ApplicationCore.Helpers.CheckBox;
    using ApplicationCore.ServiceModels.Driver;
    using ApplicationCore.Services.Mapper;
    using Infrastructure;
    using Infrastructure.Common.Enums;
    using Infrastructure.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class DriverService : IDriverService
    {
        private readonly ValkarDbContext _data;
        private readonly IMapperService _mapper;

        public DriverService(
            ValkarDbContext data,
            IMapperService mapper)
        {
            this._data = data;
            this._mapper = mapper;
        }

        public async Task AddDriver(DriverDetailsServiceModel model, string userId)
        {
            // Map driver service model to driver
            var driver = this._mapper.Map<DriverDetailsServiceModel, Driver>(model);

            // Convert DL Categories to enums and LicenceCategory models
            driver.LicenceCategories = ConvertToLicenceCategories(model.DrivingLicenceCategories);

            // set user id to this driver
            driver.UserId = userId;

            await this._data.Drivers.AddAsync(driver);
            await this._data.SaveChangesAsync();
        }

        private ICollection<LicenceCategory> ConvertToLicenceCategories(CheckBoxModel[] drivingLicenceCategories)
        {
            var licenceCategories = new List<LicenceCategory>();

            // Get only checked categories
            drivingLicenceCategories = drivingLicenceCategories
                .Where(c => c.IsChecked)
                .ToArray();

            // Make new LicenceCategories and populate licenceCategories collection
            foreach (var category in drivingLicenceCategories)
            {
                licenceCategories.Add(new LicenceCategory 
                {
                    Category = (DrivingLicenceCategories)category.Value
                });
            }
            return licenceCategories;
        }
    }
}
