namespace Web.Controllers
{
    using ApplicationCore.Helpers;
    using ApplicationCore.Helpers.CheckBox;
    using ApplicationCore.ServiceModels.Document;
    using ApplicationCore.ServiceModels.Driver;
    using ApplicationCore.Services.Driver;
    using ApplicationCore.Services.Identity;
    using Infrastructure.Common.Enums;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Web.ViewModels;

    [Authorize]
    public class DriversController : Controller
    {
        private const string YES = "Yes";

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
            return View(new DriverDetailsServiceModel()
            {
                DrivingLicenceCategories = GetDrivingLicenceCategoriesAsCheckBoxModels().ToArray(),
                Documents = new DocumentsServiceModel()
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DriverDetailsAsync(DriverDetailsServiceModel model)
        {
            ValidateDrivingLicenceCategories(model.DrivingLicenceCategories);
            if (model.IsLimitedCompany is YES)
            {
                ValidateLimitedCompanyFields(model.LimitedCompany);
            }

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
                return View(model);
            }
        }

        private List<CheckBoxModel> GetDrivingLicenceCategoriesAsCheckBoxModels()
        {
            var drivingLicenseCategories = new List<CheckBoxModel>();

            var categoryValue = 0;
            foreach (var categoryName in Enum.GetNames(typeof(DrivingLicenceCategories)))
            {
                drivingLicenseCategories.Add(new CheckBoxModel
                {
                    Text = categoryName,
                    Value = categoryValue
                });
                categoryValue++;
            }
            return drivingLicenseCategories;
        }

        private void ValidateDrivingLicenceCategories(CheckBoxModel[] drivingLicenceCategories)
        {
            if (!drivingLicenceCategories.Any(c => c.IsChecked))
            {
                ModelState.AddModelError(
                    "DrivingLicenceCategories",
                    "You should select your driving licence categories");
            }
        }

        private void ValidateLimitedCompanyFields(LimitedCompanyServiceModel limitedCompany)
        {
            if (string.IsNullOrEmpty(limitedCompany.CompanyName) ||
                string.IsNullOrWhiteSpace(limitedCompany.CompanyName))
            {
                ModelState.AddModelError(
                    "LimitedCompany.CompanyName",
                    "Company name cannot be empty.");
            }

            if (string.IsNullOrEmpty(limitedCompany.CompanyRegistrationNumber) ||
                string.IsNullOrWhiteSpace(limitedCompany.CompanyRegistrationNumber))
            {
                ModelState.AddModelError(
                    "LimitedCompany.CompanyRegistrationNumber",
                    "Company registration number cannot be empty.");
            }
        }
    }
}
