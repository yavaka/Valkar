namespace ApplicationCore.Services.Driver
{
    using ApplicationCore.Helpers.CheckBox;
    using ApplicationCore.ServiceModels.Document;
    using ApplicationCore.ServiceModels.Driver;
    using ApplicationCore.ServiceModels.WorkingDay;
    using ApplicationCore.Services.File;
    using ApplicationCore.Services.GoogleDriveAPI;
    using ApplicationCore.Services.Mapper;
    using Infrastructure;
    using Infrastructure.Common.Enums;
    using Infrastructure.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class DriverService : IDriverService
    {
        private readonly ValkarDbContext _data;
        private readonly IMapperService _mapper;
        private readonly IFileService _fileService;
        private readonly IGoogleDriveAPIService _driveAPIService;

        public DriverService(
            ValkarDbContext data,
            IMapperService mapper,
            IFileService fileService,
            IGoogleDriveAPIService driveAPIService)
        {
            this._data = data;
            this._mapper = mapper;
            this._fileService = fileService;
            this._driveAPIService = driveAPIService;
        }

        public async Task AddDriver(DriverDetailsServiceModel model, string userId)
        {
            // Map driver service model to driver
            var driver = this._mapper.Map<DriverDetailsServiceModel, Driver>(model);

            // Convert DL Categories to LicenceCategory models
            driver.LicenceCategories = ConvertToDLCategories(model.DrivingLicenceCategories);

            // Convert uploaded documents to File models
            driver.PersonalDocuments = await this._fileService
                .ProcessEmployeeUploadedDocuments(model.Documents);

            // Employee folder and documents
            driver.GoogleDriveFolderId = await ProcessGoogleDrive(model.Documents, $"{driver.FirstNames} {driver.Surname}");

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
            var driver = await this._data.Drivers
                .Include(wd => wd.WorkedDays)
                .FirstOrDefaultAsync(i => i.UserId == userId);
            var driverServiceModel = this._mapper.Map<Driver, DriverProfileServiceModel>(driver);

            foreach (var workedDay in driver.WorkedDays.OrderByDescending(d => d.Date).ToList())
            {
                driverServiceModel.WorkedDays.Add(
                    this._mapper.Map<WorkingDay, WorkingDayServiceModel>(workedDay));
            }
            return driverServiceModel;
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
                if (newDLCategories.Any(c => c.Category == category.Category) is false)
                {
                    result.Remove(category);
                }
            }

            foreach (var category in newDLCategories)
            {
                // If the category is new
                if (driverCategories.Any(i => i.Category == category.Category) is false)
                {
                    result.Add(category);
                }
            }

            return result;
        }

        private static List<LicenceCategory> ConvertToDLCategories(CheckBoxModel[] drivingLicenceCategories)
        {
            var licenceCategories = new List<LicenceCategory>();

            // Get only checked categories
            drivingLicenceCategories = drivingLicenceCategories
                .Where(c => c.IsChecked)
                .ToArray();

            // Make new LicenceCategories and populate licenceCategories collections
            foreach (var category in drivingLicenceCategories)
            {
                licenceCategories.Add(new LicenceCategory
                {
                    Category = (DrivingLicenceCategories)category.Value
                });
            }
            return licenceCategories;
        }

        /// <summary>
        /// Create new folder in google drive on employee onboarding.
        /// Upload all employee documents
        /// </summary>
        /// <returns>employee folder id</returns>
        private async Task<string> ProcessGoogleDrive(EmployeeDocumentsServiceModel documents, string fullName)
        {
            // Create folder in Google Drive for this employee
            var employeeFolderId = await this._driveAPIService.CreateFolder($"{fullName} - {Guid.NewGuid()}");

            // Upload employee documents
            
            // DL
            await this._driveAPIService.UploadFile(documents.DrivingLicenceFront, nameof(EmployeeDocumentsServiceModel.DrivingLicenceFront), employeeFolderId);
            await this._driveAPIService.UploadFile(documents.DrivingLicenceBack, nameof(EmployeeDocumentsServiceModel.DrivingLicenceBack), employeeFolderId);
            
            // ID
            await this._driveAPIService.UploadFile(documents.IdentityDocumentFront, nameof(EmployeeDocumentsServiceModel.IdentityDocumentFront), employeeFolderId);
            if (documents.IdentityDocumentBack is not null)
                await this._driveAPIService.UploadFile(documents.IdentityDocumentBack, nameof(EmployeeDocumentsServiceModel.IdentityDocumentBack), employeeFolderId);
            
            // NiNo
            await this._driveAPIService.UploadFile(documents.NationalInsuranceNumber, nameof(EmployeeDocumentsServiceModel.NationalInsuranceNumber), employeeFolderId);

            return employeeFolderId;
        }

        #endregion
    }
}
