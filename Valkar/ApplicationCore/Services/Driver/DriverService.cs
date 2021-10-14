namespace ApplicationCore.Services.Driver
{
    using ApplicationCore.Helpers.CheckBox;
    using ApplicationCore.ServiceModels.Admin;
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

        public async Task UpdateDriverDetails(UpdateDriverDetailsServiceModel model, string userId)
        {
            // Get the driver entity and its DL categories by current user id
            var driver = await this._data.Drivers
                .Include(lc => lc.LicenceCategories)
                .FirstOrDefaultAsync(i => i.UserId == userId);

            // Set only the different props to the driver entity
            this._data.Entry(driver).CurrentValues.SetValues(model);

            // Update driving licence categories
            driver.LicenceCategories = UpdateDLCategories(
                model.DrivingLicenceCategories,
                driver.LicenceCategories.ToList());

            await this._data.SaveChangesAsync();
        }

        public async Task UpdateLimitedCompany(LimitedCompanyServiceModel model, string userId)
        {
            var limitedCompany = await this._data.Drivers
                .Include(l => l.LimitedCompany)
                .Where(uId => uId.UserId == userId)
                .Select(l => l.LimitedCompany)
                .FirstOrDefaultAsync();

            this._data.Entry(limitedCompany).CurrentValues.SetValues(model);

            await this._data.SaveChangesAsync();
        }

        public async Task<SettingsServiceModel> GetDriverSettingsByUserId(string userId)
        {
            // Get driver with its DL categories and limited company
            var driver = await this._data.Drivers
                .Include(l => l.LicenceCategories)
                .Include(lc => lc.LimitedCompany)
                .FirstOrDefaultAsync(i => i.UserId == userId);

            var settingsModel = new SettingsServiceModel();

            settingsModel.DriverDetails = this._mapper
                .Map<Driver, UpdateDriverDetailsServiceModel>(driver);

            settingsModel.LimitedCompany = this._mapper
                .Map<LimitedCompany, LimitedCompanyServiceModel>(driver.LimitedCompany);

            settingsModel.DriverDetails.DrivingLicenceCategories = Converter
                .GetDrivingLicenceCategoriesAsCheckBoxModels(driver.LicenceCategories.ToList());

            return settingsModel;
        }

        public async Task<UpdateDriverDetailsServiceModel> GetDriverDetailsForUpdateByUserId(string userId)
        {
            // Get driver with its DL categories and limited company
            var driver = await this._data.Drivers
                .Include(l => l.LicenceCategories)
                .FirstOrDefaultAsync(i => i.UserId == userId);

            var driverDetails = this._mapper.Map<Driver, UpdateDriverDetailsServiceModel>(driver);

            // Add licence categories
            driverDetails.DrivingLicenceCategories = Converter
                .GetDrivingLicenceCategoriesAsCheckBoxModels(driver.LicenceCategories.ToList());

            return driverDetails;
        }

        public async Task<LimitedCompanyServiceModel> GetLimitedCompanyByUserId(string userId)
        {
            // Get limited company
            var limitedCompany = await this._data.Drivers
                .Include(lc => lc.LimitedCompany)
                .Where(i => i.UserId == userId)
                .Select(lc => lc.LimitedCompany)
                .FirstAsync();

            // Map driver details
            return this._mapper.Map<LimitedCompany, LimitedCompanyServiceModel>(limitedCompany);
        }

        public async Task<DriverProfileServiceModel> GetDriverProfileByUserId(string userId)
        {
            // Get driver with its DL categories and limited company
            var driver = await this._data.Drivers
                .FirstOrDefaultAsync(i => i.UserId == userId);

            return this._mapper.Map<Driver, DriverProfileServiceModel>(driver);
        }

        #region Helpers

        private List<LicenceCategory> UpdateDLCategories(CheckBoxModel[] dLCategoriesCheckBoxes, List<LicenceCategory> driverCategories)
        {
            // Get the new driving licence categories
            var newDLCategories = ConvertToDLCategories(dLCategoriesCheckBoxes);

            // Backup collection which is not tracked by the EF Core
            var result = driverCategories.ToList();

            foreach (var category in driverCategories)
            {
                // If the category is removed
                if (!newDLCategories.Any(c => c.Category == category.Category))
                {
                    result.Remove(category);
                }
            }

            foreach (var category in newDLCategories)
            {
                // If the category is new
                if (!driverCategories.Any(i => i.Category == category.Category))
                {
                    result.Add(category);
                }
            }

            return result;
        }

        private List<LicenceCategory> ConvertToDLCategories(CheckBoxModel[] drivingLicenceCategories)
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

        #endregion
    }
}
