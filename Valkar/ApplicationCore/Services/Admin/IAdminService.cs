namespace ApplicationCore.Services.Admin
{
    using ApplicationCore.ServiceModels.Admin;
    using System.Collections.Generic;

    public interface IAdminService
    {
        IEnumerable<DriverAdminServiceModel> GetAllDrivers();
        DriverAdminServiceModel GetDriverProfile(string userId);
    }
}
