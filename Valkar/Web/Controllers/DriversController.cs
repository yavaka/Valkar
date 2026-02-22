namespace Web.Controllers
{
    using ApplicationCore.Helpers;
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
    using System.Linq;
    using System.Threading.Tasks;
    using static Infrastructure.Common.ModelConstants;

    [Authorize(Roles = Role.Driver)]
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
        public async Task<IActionResult> Profile()
        {
            // Redirect to onboarding view if not completed
            var userId = this._identityService.GetUserId(User);
            if (await this._identityService.IsOnboardingCompleted(userId) is false)
            {
                return RedirectToAction(nameof(DriverDetails));
            }
            var driver = await this._driverService.GetDriverProfileByUserId(userId);
            return View(driver);
        }

        [HttpGet]
        public async Task<IActionResult> Settings(SettingsServiceModel model = null)
        {
            // Redirect to onboarding view if not completed
            var userId = this._identityService.GetUserId(User);
            if (await this._identityService.IsOnboardingCompleted(userId) is false)
            {
                return RedirectToAction(nameof(DriverDetails));
            }

            // Get driver details and limited company 
            var driverSettings = await this._driverService
                .GetDriverSettingsByUserId(userId);

            return View(driverSettings);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateDriverSettings(UpdateDriverDetailsServiceModel model)
        {
            // Get current user id
            var userId = this._identityService.GetUserId(User);

            if (ModelState.IsValid)
            {
                // Update driver details without any other entities
                await this._driverService.UpdateDriverDetails(model, userId);

                TempData["updatedDriverDetailsAlert"] = SUCCESSFUL_UPDATE_MSG;

                return RedirectToAction(nameof(Settings));
            }
            return View(nameof(Settings),
                new SettingsServiceModel
                { // Return driver details model errors and ltd fields
                    DriverDetails = model,
                    LimitedCompany = await this._driverService.GetLimitedCompanyByUserId(userId)
                });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateLimitedCompany(LimitedCompanyServiceModel model)
        {
            // Get the current user id
            var userId = this._identityService.GetUserId(User);

            ValidateUpdatedLimitedCompanyFields(model);

            if (ModelState.IsValid)
            {
                // Update driver`s limited company
                await this._driverService.UpdateLimitedCompany(model, userId);

                TempData["updatedDriverDetailsAlert"] = SUCCESSFUL_UPDATE_MSG;

                return RedirectToAction(nameof(Settings));
            }
            return View(nameof(Settings),
                new SettingsServiceModel
                { // Return ltd model errors and driver details fields
                    LimitedCompany = model,
                    DriverDetails = await this._driverService.GetDriverDetailsForUpdateByUserId(userId)
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
            Validations(model);
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

        #region Validations

        private void Validations(DriverDetailsServiceModel model)
        {
            ValidateDriverDetails(model);
            ValidateDrivingLicenceCategories(model.DrivingLicenceCategories);
            ValidateUploadedFiles(model.Documents);
            ValidateEmergencyContact(model.EmergencyContact);
        }

        private void ValidateDriverDetails(DriverDetailsServiceModel model)
        {
            if (model.Title == Titles.None)
            {
                ModelState.AddModelError("Title", "Title required");
            }
            if (string.IsNullOrEmpty(model.FirstNames) || (model.FirstNames.Length > MAX_NAME_LENGTH || model.FirstNames.Length < MIN_NAME_LENGTH))
            {
                ModelState.AddModelError("FirstNames", $"First name/s cannot be less than {MIN_NAME_LENGTH} and more than {MAX_NAME_LENGTH} symbols");
            }
            if (string.IsNullOrEmpty(model.Surname) || (model.Surname.Length > MAX_NAME_LENGTH || model.Surname.Length < MIN_NAME_LENGTH))
            {
                ModelState.AddModelError("Surname", $"Surname cannot be less than {MIN_NAME_LENGTH} and more than {MAX_NAME_LENGTH} symbols");
            }
            if (string.IsNullOrEmpty(model.Address) || (model.Address.Length > MAX_ADDRESS_LENGTH))
            {
                ModelState.AddModelError("Address", $"Address cannot be more than {MAX_ADDRESS_LENGTH} symbols");
            }
            if (string.IsNullOrEmpty(model.Postcode) || (ValidationHelper.RegexValidation(model.Postcode, POSTCODE_REGEX) is false))
            {
                ModelState.AddModelError("Postcode", $"Invalid Postcode");
            }
            ValidateDateOfBirth(model.DateOfBirth);
            if (string.IsNullOrEmpty(model.PhoneNumber) || (ValidationHelper.RegexValidation(model.PhoneNumber, PHONE_NUMBER_REGEX) is false))
            {
                ModelState.AddModelError("PhoneNumber", $"Invalid Phone number");
            }
            if (string.IsNullOrEmpty(model.NationalInsuranceNumber))
            {
                ModelState.AddModelError("NationalInsuranceNumber", $"Invalid National insurance number");
            }
        }

        private void ValidateDateOfBirth(DateTime dateOfBirth)
        {
            var today = DateTime.Today;
            var age = today.Year - dateOfBirth.Year;
            if (dateOfBirth > today.AddYears(-age))
            {
                age--;
            }
            if (age < MIN_DRIVER_AGE)
            {
                ModelState.AddModelError("DateOfBirth", "Age is under 18");
            }
        }

        private void ValidateDrivingLicenceCategories(CheckBoxModel[] drivingLicenceCategories)
        {
            if (drivingLicenceCategories.Any(c => c.IsChecked) is false)
            {
                ModelState.AddModelError(
                    "DrivingLicenceCategories",
                    "Driving licence categories required");
            }
        }

        /// <summary>
        /// Validate the ltd fields from settings page
        /// </summary>
        private void ValidateUpdatedLimitedCompanyFields(LimitedCompanyServiceModel model)
        {
            if (string.IsNullOrEmpty(model.CompanyName) is false &&
                string.IsNullOrEmpty(model.CompanyRegistrationNumber))
            {
                ModelState.AddModelError("CompanyRegistrationNumber", "Company registration number required.");
            }
            if (string.IsNullOrEmpty(model.CompanyName) &&
                string.IsNullOrEmpty(model.CompanyRegistrationNumber) is false)
            {
                ModelState.AddModelError("CompanyName", "Company name required.");
            }
        }

        private void ValidateUploadedFiles(EmployeeDocumentsServiceModel documents)
        {
            // DL front
            if (documents.DrivingLicenceFront is null)
            {
                ModelState.AddModelError("Documents.DrivingLicenceFront", $"Driving Licence is required!");
            }
            else
            {
                if (documents.DrivingLicenceFront.Length > MAX_FILE_SIZE)
                {
                    ModelState.AddModelError("Documents.DrivingLicenceFront", $"File cannot be more than 20MB.");
                }
                else if (ValidationHelper.RegexValidation(documents.DrivingLicenceFront.FileName.ToLower(), FILE_EXTENSIONS_REGEX) is false)
                {
                    ModelState.AddModelError("Documents.DrivingLicenceFront", $"Invalid file, allowed file extensions are .jpg, .jpeg, .png, .bmp, .pdf, .doc, .docx");
                }
            }

            // DL back
            if (documents.DrivingLicenceBack is null)
            {
                ModelState.AddModelError("Documents.DrivingLicenceBack", $"Driving Licence is required!");
            }
            else
            {
                if (documents.DrivingLicenceBack.Length > MAX_FILE_SIZE)
                {
                    ModelState.AddModelError("Documents.DrivingLicenceBack", $"File cannot be more than 20MB.");
                }
                else if (ValidationHelper.RegexValidation(documents.DrivingLicenceBack.FileName.ToLower(), FILE_EXTENSIONS_REGEX) is false)
                {
                    ModelState.AddModelError("Documents.DrivingLicenceBack", $"Invalid file, allowed file extensions are .jpg, .jpeg, .png, .bmp, .pdf, .doc, .docx");
                }
            }

            // ID front
            if (documents.IdentityDocumentFront is null)
            {
                ModelState.AddModelError("Documents.IdentityDocumentFront", $"Identity Document is required!");
            }
            else
            {
                if (documents.IdentityDocumentFront.Length > MAX_FILE_SIZE)
                {
                    ModelState.AddModelError("Documents.IdentityDocumentFront", $"File cannot be more than 20MB.");
                }
                else if (ValidationHelper.RegexValidation(documents.IdentityDocumentFront.FileName.ToLower(), FILE_EXTENSIONS_REGEX) is false)
                {
                    ModelState.AddModelError("Documents.IdentityDocumentFront", $"Invalid file, allowed file extensions are .jpg, .jpeg, .png, .bmp, .pdf, .doc, .docx");
                }
            }

            // ID back
            if (documents.IdentityDocumentBack is not null && documents.IdentityDocumentBack.Length > MAX_FILE_SIZE)
            {
                ModelState.AddModelError("Documents.IdentityDocumentBack", $"File cannot be more than 20MB.");
            }
            else if (documents.IdentityDocumentBack is not null && ValidationHelper.RegexValidation(documents.IdentityDocumentBack.FileName.ToLower(), FILE_EXTENSIONS_REGEX) is false)
            {
                ModelState.AddModelError("Documents.IdentityDocumentBack", $"Invalid file, allowed file extensions are .jpg, .jpeg, .png, .bmp, .pdf, .doc, .docx");
            }

            // NiNo letter
            if (documents.NationalInsuranceNumber is null)
            {
                ModelState.AddModelError("Documents.NationalInsuranceNumber", $"NiNo letter is required!");
            }
            else
            {
                if (documents.NationalInsuranceNumber.Length > MAX_FILE_SIZE)
                {
                    ModelState.AddModelError("Documents.NationalInsuranceNumber", $"File cannot be more than 20MB.");
                }
                else if (ValidationHelper.RegexValidation(documents.NationalInsuranceNumber.FileName.ToLower(), FILE_EXTENSIONS_REGEX) is false)
                {
                    ModelState.AddModelError("Documents.NationalInsuranceNumber", $"Invalid file, allowed file extensions are .jpg, .jpeg, .png, .bmp, .pdf, .doc, .docx");
                }
            }
        }

        private void ValidateEmergencyContact(EmergencyContactServiceModel model)
        {
            if (model.Title == Titles.None)
            {
                ModelState.AddModelError("EmergencyContact.Title", "Title required");
            }
            if (string.IsNullOrEmpty(model.FirstNames) || (model.FirstNames.Length > MAX_NAME_LENGTH || model.FirstNames.Length < MIN_NAME_LENGTH))
            {
                ModelState.AddModelError("EmergencyContact.FirstNames", $"First name/s cannot be less than {MIN_NAME_LENGTH} and more than {MAX_NAME_LENGTH} symbols");
            }
            if (string.IsNullOrEmpty(model.Surname) || (model.Surname.Length > MAX_NAME_LENGTH || model.Surname.Length < MIN_NAME_LENGTH))
            {
                ModelState.AddModelError("EmergencyContact.Surname", $"Surname cannot be less than {MIN_NAME_LENGTH} and more than {MAX_NAME_LENGTH} symbols");
            }
            if (string.IsNullOrEmpty(model.Address) || (model.Address.Length > MAX_ADDRESS_LENGTH))
            {
                ModelState.AddModelError("EmergencyContact.Address", $"Address cannot be more than {MAX_ADDRESS_LENGTH} symbols");
            }
            if (string.IsNullOrEmpty(model.Postcode) || (ValidationHelper.RegexValidation(model.Postcode, POSTCODE_REGEX) is false))
            {
                ModelState.AddModelError("EmergencyContact.Postcode", $"Invalid Postcode");
            }
            if (string.IsNullOrEmpty(model.Email) || (ValidationHelper.RegexValidation(model.Email, EMAIL_REGEX) is false))
            {
                ModelState.AddModelError("EmergencyContact.Email", $"Invalid Email");
            }
            if (string.IsNullOrEmpty(model.PhoneNumber) || (ValidationHelper.RegexValidation(model.PhoneNumber, PHONE_NUMBER_REGEX) is false))
            {
                ModelState.AddModelError("EmergencyContact.PhoneNumber", $"Invalid Phone number");
            }
            if (string.IsNullOrEmpty(model.Relationship) || (model.Relationship.Length > MAX_NAME_LENGTH || model.Relationship.Length < MIN_NAME_LENGTH))
            {
                ModelState.AddModelError("EmergencyContact.Relationship", $"Relationship cannot be less than {MIN_NAME_LENGTH} and more than {MAX_NAME_LENGTH} symbols");
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

        #endregion
    }
}
