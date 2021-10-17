namespace Web.Controllers
{
    using ApplicationCore.ServiceModels.WorkingDay;
    using ApplicationCore.Services.WorkingDay;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Threading.Tasks;

    public class WorkingDaysController : Controller
    {
        private readonly IWorkingDayService _workingDayService;

        public WorkingDaysController(IWorkingDayService workingDayService)
            => this._workingDayService = workingDayService;

        [HttpGet]
        public IActionResult AddWorkingDay(string driverId)
        {
            return View(new WorkingDayServiceModel()
            {
                DriverId = driverId
            });
        }

        [HttpPost]
        public async Task<IActionResult> AddWorkingDayAsync(WorkingDayServiceModel model)
        {
            Validate(model);

            if (ModelState.IsValid)
            {
                model.CalculateTotlaHours();
                await this._workingDayService.AddWorkingDay(model);
                return RedirectToAction("Profile", "Drivers");
            }
            return View(model);
        }

        private void Validate(WorkingDayServiceModel model)
        {
            if (model.Date == default)
            {
                ModelState.AddModelError(nameof(WorkingDayServiceModel.Date), $"{nameof(WorkingDayServiceModel.Date)} is required");
            }
            if (model.Date > DateTime.Now)
            {
                ModelState.AddModelError(nameof(WorkingDayServiceModel.Date), $"{nameof(WorkingDayServiceModel.Date)} cannot be in the future");
            }
            if (model.TimeOut == default)
            {
                ModelState.AddModelError(nameof(WorkingDayServiceModel.TimeOut), $"{nameof(WorkingDayServiceModel.TimeOut)} is required");
            }
            if (model.TimeOut < model.TimeIn)
            {
                ModelState.AddModelError(nameof(WorkingDayServiceModel.TimeOut), $"{nameof(WorkingDayServiceModel.TimeOut)} cannot be less than {nameof(WorkingDayServiceModel.TimeIn)}");
            }
            if (model.Break > model.TimeOut.Subtract(model.TimeIn))
            {
                ModelState.AddModelError(nameof(WorkingDayServiceModel.Break), $"{nameof(WorkingDayServiceModel.Break)} cannot be more than worked hours");
            }
        }
    }
}
