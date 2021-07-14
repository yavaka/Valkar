namespace Web.Controllers
{
    using ApplicationCore.Helpers;
    using ApplicationCore.ServiceModels.Driver;
    using ApplicationCore.Services.Driver;
    using ApplicationCore.Services.Identity;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    [Authorize]
    public class DriversController : Controller
    {
        private readonly IDriverService _driverService;
        private readonly IIdentityService _identityService;

        public DriversController(
            IDriverService driverService,
            IIdentityService identityService)
        {
            this._driverService = driverService;
            this._identityService = identityService;
        }

        [HttpGet]
        public IActionResult Profile()
        {
            return View();
        }

        [HttpGet]
        public IActionResult DriverDetails()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DriverDetailsAsync(DriverDetailsServiceModel model)
        {
            if (ModelState.IsValid)
            {
                // Get current user id
                var userId = this._identityService.GetUserId(User);

                // Add driver details and
                // establish the relationship between the driver and the current user
                await this._driverService.AddDriver(model, userId);

                // Update current user that the driver already completed the onboarding
                await this._identityService.CompleteOnboarding(userId);

                return RedirectToAction(nameof(Profile));
            }
            else
            {
                var error = ModelState.Values;
                return View(model);
            }
        }
    }
}
