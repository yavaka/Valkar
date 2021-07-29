﻿namespace ApplicationCore.Services.Driver
{
    using ApplicationCore.Helpers.CheckBox;
    using ApplicationCore.ServiceModels.Driver;
    using ApplicationCore.Services.File;
    using ApplicationCore.Services.Mapper;
    using Infrastructure;
    using Infrastructure.Common.Enums;
    using Infrastructure.Models;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class DriverService : IDriverService
    {
        private readonly ValkarDbContext _data;
        private readonly IMapperService _mapper;
        private readonly IFileService _fileService;

        public DriverService(
            ValkarDbContext data,
            IMapperService mapper,
            IFileService fileService)
        {
            this._data = data;
            this._mapper = mapper;
            this._fileService = fileService;
        }

        public async Task AddDriver(DriverDetailsServiceModel model, string userId)
        {
            // Map driver service model to driver
            var driver = this._mapper.Map<DriverDetailsServiceModel, Driver>(model);

            // Convert DL Categories to LicenceCategory models
            driver.LicenceCategories = ConvertToDLCategories(model.DrivingLicenceCategories);

            // Convert uploaded documents to File models
            driver.Documents = await this._fileService
                .ProcessUploadedDocuments(model.Documents);

            // set user id to this driver
            driver.UserId = userId;

            await this._data.Drivers.AddAsync(driver);
            await this._data.SaveChangesAsync();
        }

        public async Task<SettingsServiceModel> GetDriverSettingsByUserId(string userId)
        {
            var driver = await this._data.Drivers
                .Include(l => l.LicenceCategories)
                .Include(lc => lc.LimitedCompany)
                .FirstOrDefaultAsync(i => i.UserId == userId);

            var driverSettings = this._mapper
                .Map<Driver, SettingsServiceModel>(driver);

            driverSettings.LimitedCompany = this._mapper
                .Map<LimitedCompany, LimitedCompanyServiceModel>(driver.LimitedCompany);

            driverSettings.DrivingLicenceCategories = Converter
                .GetDrivingLicenceCategoriesAsCheckBoxModels(driver.LicenceCategories.ToList());

            return driverSettings;
        }

        private ICollection<LicenceCategory> ConvertToDLCategories(CheckBoxModel[] drivingLicenceCategories)
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
