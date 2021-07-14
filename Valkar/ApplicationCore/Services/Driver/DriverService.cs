namespace ApplicationCore.Services.Driver
{
    using ApplicationCore.Helpers;
    using ApplicationCore.ServiceModels.Driver;
    using ApplicationCore.Services.Mapper;
    using Infrastructure;
    using Infrastructure.Models;
    using System;
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
            // set user id to this driver
            driver.UserId = userId;

            await this._data.Drivers.AddAsync(driver);
            await this._data.SaveChangesAsync();
        }
    }
}
