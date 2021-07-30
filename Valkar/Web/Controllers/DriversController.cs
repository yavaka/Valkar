namespace Web.Controllers
{
    using ApplicationCore.Helpers.CheckBox;
    using ApplicationCore.ServiceModels.Document;
    using ApplicationCore.ServiceModels.Driver;
    using ApplicationCore.Services.Driver;
    using ApplicationCore.Services.Identity;
    using Infrastructure.Common.Global;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Linq;
    using System.Threading.Tasks;
    using Web.Extensions;

    [Authorize(Roles = Role.Driver)]
    public class DriversController : Controller
    {
        private const string YES = "Yes";
        private const long MAX_FILE_SIZE = 10 * 1024 * 1024;
        private const string SUCCESSFUL_UPDATE_MSG = "Your details was updated successfully.";

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

            // Get driver details and limited company 
            var driverSettings = await this._driverService
                .GetDriverSettingsByUserId(
                    this._identityService.GetUserId(User));

            return View(driverSettings);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateDriverSettings(UpdateDriverDetailsServiceModel model)
        {
            if (ModelState.IsValid)
            {
                // Get current user id
                var userId = this._identityService.GetUserId(User);

                // Update driver details without any other entities
                await this._driverService.UpdateDriverDetails(model, userId);

                TempData["updatedDriverDetailsAlert"] = SUCCESSFUL_UPDATE_MSG;

                return RedirectToAction(nameof(Settings));
            }
            // Get the limited company service model set in Settings view 
            var limitedCompany = TempData.Get<LimitedCompanyServiceModel>("limitedCompany");
            TempData.Remove("limitedCompany");

            return View(nameof(Settings), new SettingsServiceModel
            {
                DriverDetails = model,
                LimitedCompany = limitedCompany
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateLimitedCompany(LimitedCompanyServiceModel model)
        {
            ValidateUpdatedLimitedCompanyFields(model);

            if (ModelState.IsValid)
            {
                // Get the current user
                var userId = this._identityService.GetUserId(User);

                // Update driver`s limited company
                await this._driverService.UpdateLimitedCompany(model, userId);

                TempData["updatedDriverDetailsAlert"] = SUCCESSFUL_UPDATE_MSG;

                return RedirectToAction(nameof(Settings));
            }
            // Get the update driver details service model set in Settings view 
            var driverDetails = TempData.Get<UpdateDriverDetailsServiceModel>("driverDetails");
            TempData.Remove("driverDetails");

            return View(nameof(Settings), new SettingsServiceModel
            {
                LimitedCompany = model,
                DriverDetails = driverDetails
            });
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

        /// <summary>
        /// Validate the ltd fields from settings page
        /// </summary>
        private void ValidateUpdatedLimitedCompanyFields(LimitedCompanyServiceModel model)
        {
            if (!string.IsNullOrEmpty(model.CompanyName) &&
                string.IsNullOrEmpty(model.CompanyRegistrationNumber))
            {
                ModelState.AddModelError("CompanyRegistrationNumber", "Company registration number required.");
            }
            if (string.IsNullOrEmpty(model.CompanyName) &&
                !string.IsNullOrEmpty(model.CompanyRegistrationNumber))
            {
                ModelState.AddModelError("CompanyName", "Company name required.");
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
