namespace Web.Controllers
{
    using ApplicationCore.Helpers;
    using ApplicationCore.ServiceModels.Company;
    using ApplicationCore.Services.Company;
    using Infrastructure.Common.Global;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using static Infrastructure.Common.ModelConstants;

    [Authorize(Roles = Role.Admin)]
    public class CompaniesController : Controller
    {
        private readonly ICompanyService _companyService;

        public CompaniesController(ICompanyService companyService)
        {
            this._companyService = companyService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var companies = await this._companyService.FetchAllAsync();
            return View(companies);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(CompanyServiceModel model)
        {
            Validate(model);
            if (ModelState.IsValid)
            {
                await this._companyService.AddAsync(model);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            return View(await this._companyService.FetchAsync(id));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            return View(await this._companyService.FetchAsync(id));
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CompanyServiceModel model)
        {
            Validate(model);
            if (ModelState.IsValid)
            {
                await this._companyService.UpdateAsync(model);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmDeletion(string id)
        {
            return PartialView("CompanyPartials/_DeleteCompanyConfirmation", await this._companyService.FetchAsync(id));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            await this._companyService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        #region Validation

        private void Validate(CompanyServiceModel model)
        {
            if (model is not null)
            {
                if (string.IsNullOrEmpty(model.Name) || (model.Name.Length > MAX_COMPANY_NAME_LENGTH || model.Name.Length < MIN_COMPANY_NAME_LENGTH))
                    ModelState.AddModelError("Name", $"Company name cannot be less than {MIN_COMPANY_NAME_LENGTH} and more than {MAX_COMPANY_NAME_LENGTH} symbols");
                if (string.IsNullOrEmpty(model.EmailAddress) || (!ValidationHelper.RegexValidation(model.EmailAddress, EMAIL_REGEX)))
                    ModelState.AddModelError("EmailAddress", $"Invalid Email address");
                if (string.IsNullOrEmpty(model.PhoneNumber) || (!ValidationHelper.RegexValidation(model.PhoneNumber, PHONE_NUMBER_REGEX)))
                    ModelState.AddModelError("PhoneNumber", $"Invalid Phone number");
                
                if (string.IsNullOrEmpty(model.RegistrationNumber) is false 
                    && (model.RegistrationNumber.Length < FIXED_COMPANY_REGISTRATION_NUMBER || model.RegistrationNumber.Length > FIXED_COMPANY_REGISTRATION_NUMBER
                    || model.RegistrationNumber.ToCharArray().Any(c => char.IsDigit(c) is false)))
                    ModelState.AddModelError("RegistrationNumber", $"Company registration number is fixed {FIXED_COMPANY_REGISTRATION_NUMBER} digits");
                
                if (string.IsNullOrEmpty(model.OfficeAddress) || (model.OfficeAddress.Length > MAX_ADDRESS_LENGTH))
                    ModelState.AddModelError("OfficeAddress", $"Address cannot be more than {MAX_ADDRESS_LENGTH} symbols");
                if (string.IsNullOrEmpty(model.OfficePostCode) || (!ValidationHelper.RegexValidation(model.OfficePostCode, POSTCODE_REGEX)))
                    ModelState.AddModelError("OfficePostCode", $"Invalid Postcode");
            }
            else
                ModelState.AddModelError("errors", "Internal error. Contact administrator.");
        }

        #endregion
    }
}
