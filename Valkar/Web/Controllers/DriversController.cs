﻿namespace Web.Controllers
{
    using ApplicationCore.Helpers.CheckBox;
    using ApplicationCore.ServiceModels.Document;
    using ApplicationCore.ServiceModels.Driver;
    using ApplicationCore.ServiceModels.Identity;
    using ApplicationCore.Services.Driver;
    using ApplicationCore.Services.Identity;
    using Infrastructure.Common.Global;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Linq;
    using System.Threading.Tasks;

    [Authorize(Roles = Role.Driver)]
    public class DriversController : Controller
    {
        private const string YES = "Yes";
        private const long MAX_FILE_SIZE = 10 * 1024 * 1024;

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
        public async Task<IActionResult> Profile()
        {
            // Redirect to onboarding view if not completed
            var userId = this._identityService.GetUserId(User);
            if (!await this._identityService.IsOnboardingCompleted(userId))
            {
                return RedirectToAction(nameof(DriverDetails));
            }

            return View(new DriverDetailsServiceModel { FirstNames = "TestUser" });
        }

        [HttpGet]
        public async Task<IActionResult> Settings()
        {
            // Redirect to onboarding view if not completed
            var userId = this._identityService.GetUserId(User);
            if (!await this._identityService.IsOnboardingCompleted(userId))
            {
                return RedirectToAction(nameof(DriverDetails));
            }

            var driverSettings = await this._driverService
                .GetDriverSettingsByUserId(
                    this._identityService.GetUserId(User));

            return View(driverSettings);
        }

        [HttpGet]
        public async Task<IActionResult> DriverDetails()
        {
            // Redirect to driver profile if onboarding completed
            var userId = this._identityService.GetUserId(User);
            if (await this._identityService.IsOnboardingCompleted(userId))
            {
                return RedirectToAction(nameof(Profile));
            }

            return View(new DriverDetailsServiceModel()
            {
                DrivingLicenceCategories = Converter.GetDrivingLicenceCategoriesAsCheckBoxModels(),
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DriverDetails(DriverDetailsServiceModel model)
        {
            // TODO: Define Validation method and bring all validation logic there
            ValidateUploadedFiles(model.Documents);
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

        private void ValidateUploadedFiles(DocumentsServiceModel documents)
        {
            if (documents.DrivingLicenceFront.Length > MAX_FILE_SIZE)
            {
                ModelState.AddModelError("Documents.DrivingLicenceFront", $"File cannot be more than 10MB.");
            }
            if (documents.DrivingLicenceBack.Length > MAX_FILE_SIZE)
            {
                ModelState.AddModelError("Documents.DrivingLicenceBack", $"File cannot be more than 10MB.");
            }
            if (documents.IdentityDocumentFront.Length > MAX_FILE_SIZE)
            {
                ModelState.AddModelError("Documents.IdentityDocumentFront", $"File cannot be more than 10MB.");
            }
            if (documents.IdentityDocumentBack is not null && documents.IdentityDocumentBack.Length > MAX_FILE_SIZE)
            {
                ModelState.AddModelError("Documents.IdentityDocumentBack", $"File cannot be more than 10MB.");
            }
            if (documents.NationalInsuranceNumber.Length > MAX_FILE_SIZE)
            {
                ModelState.AddModelError("Documents.NationalInsuranceNumber", $"File cannot be more than 10MB.");
            }
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
