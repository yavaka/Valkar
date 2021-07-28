namespace Web.Controllers
{
    using ApplicationCore.Helpers.CheckBox;
    using ApplicationCore.ServiceModels.Document;
    using ApplicationCore.ServiceModels.Driver;
    using ApplicationCore.Services.Driver;
    using ApplicationCore.Services.Identity;
    using Infrastructure.Common.Enums;
    using Infrastructure.Common.Global;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
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
