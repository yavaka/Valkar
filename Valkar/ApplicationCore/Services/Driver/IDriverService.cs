﻿namespace ApplicationCore.Services.Driver
{
    using ApplicationCore.ServiceModels.Driver;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IDriverService
    {
        Task AddDriver(DriverDetailsServiceModel model, string userId);

        Task UpdateDriverDetails(UpdateDriverDetailsServiceModel model, string userId);

        Task UpdateLimitedCompany(LimitedCompanyServiceModel model, string userId);

        Task<SettingsServiceModel> GetDriverSettingsByUserId(string userId);

        Task<UpdateDriverDetailsServiceModel> GetDriverDetailsForUpdateByUserId(string userId);

        Task<LimitedCompanyServiceModel> GetLimitedCompanyByUserId(string userId);
        
        Task<DriverProfileServiceModel> GetDriverProfileByUserId(string userId);
    }
}
