namespace Web.Controllers
{
    using ApplicationCore.ServiceModels.Company;
    using ApplicationCore.Services.Company;
    using Infrastructure.Common.Global;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;

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
            await this._companyService.AddAsync(model);
            return RedirectToAction(nameof(Index));
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
            await this._companyService.UpdateAsync(model);
            return RedirectToAction(nameof(Index));
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
    }
}
