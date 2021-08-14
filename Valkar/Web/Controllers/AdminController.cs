namespace Web.Controllers
{
    using ApplicationCore.ServiceModels.Admin;
    using ApplicationCore.Services.Admin;
    using ApplicationCore.Services.Driver;
    using Infrastructure.Common.Global;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;

    [Authorize(Roles = Role.Admin)]
    public class AdminController : Controller
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            this._adminService = adminService;
        }

        public IActionResult Dashboard()
        {
            return View(new DashboardServiceModel
            {
                Drivers = this._adminService.GetAllDrivers()
            });
        }

        public IActionResult DriverProfile(string userId) 
        {
            return View(this._adminService.GetDriverProfile(userId));
        }
    }
}
