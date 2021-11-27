namespace Web.Controllers
{
    using ApplicationCore.ServiceModels.Admin;
    using ApplicationCore.Services.Admin;
    using ApplicationCore.Services.Driver;
    using Infrastructure.Common.Global;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Linq;
    using System.Threading.Tasks;

    [Authorize(Roles = Role.Admin)]
    public class AdminController : Controller
    {
        private readonly IAdminService _adminService;
        private readonly IDriverService _driverService;

        public AdminController(IAdminService adminService,
            IDriverService driverService)
        {
            this._adminService = adminService;
            this._driverService = driverService;
        }

        public IActionResult Dashboard()
        {
            return View(new DashboardServiceModel
            {
                Drivers = this._adminService
                    .GetAllDrivers()
                    .OrderBy(f => f.FirstNames)
            });
        }

        public async Task<IActionResult> DriverProfileAsync(string userId) 
        {
            return View(await this._adminService.GetDriverProfileAsync(userId));
        }
    }
}
