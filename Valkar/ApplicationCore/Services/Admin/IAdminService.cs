namespace ApplicationCore.Services.Admin
{
    using ApplicationCore.ServiceModels.Admin;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IAdminService
    {
        IEnumerable<DriverAdminServiceModel> GetAllDrivers();
        Task<DriverAdminServiceModel> GetDriverProfileAsync(string userId);
    }
}
