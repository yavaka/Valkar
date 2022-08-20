using ApplicationCore.ServiceModels.Report;
using ApplicationCore.Services.Report;
using Infrastructure.Common.Global;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Web.Controllers
{
    [Authorize(Roles = Role.Admin)]
    public class ReportsController : Controller
    {
        private readonly IReportService _reportService;

        public ReportsController(IReportService reportService)
            => this._reportService = reportService;

        [HttpGet]
        public IActionResult GenerateTimeSheetReport(string employeeId)
        {
            return View(new TimeSheetReportServiceModel
            {
                EmployeeId = employeeId
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GenerateTimeSheetReport(TimeSheetReportServiceModel viewModel)
        {
            ValidateTimeSheetReport(viewModel);

            if (ModelState.IsValid)
                return File(await this._reportService.GenerateTimeSheetReport(viewModel), "application/pdf");

            return View(viewModel);
        }

        #region Validations

        // NOTE: Change to generic if more than one reports
        private void ValidateTimeSheetReport(TimeSheetReportServiceModel viewModel)
        {
            // TODO: Log exception
            if (viewModel is null || string.IsNullOrEmpty(viewModel.EmployeeId))
                ModelState.AddModelError(nameof(TimeSheetReportServiceModel.Error), "Error code {BUG-01} - Time Sheet report generation failed. Contact Administration department and provide value in curly brackets.");

            if (viewModel.StartDate > DateTime.Today.Date)
            {
                ModelState.AddModelError(nameof(TimeSheetReportServiceModel.StartDate), "Date cannot be in the future.");
            }
            if (string.IsNullOrEmpty(viewModel.Company))
            {
                ModelState.AddModelError(nameof(TimeSheetReportServiceModel.Company), "Please select company.");
            }
        }

        #endregion
    }
}
