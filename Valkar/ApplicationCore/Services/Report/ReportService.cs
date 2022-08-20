namespace ApplicationCore.Services.Report
{
    using ApplicationCore.ServiceModels.Report;
    using AspNetCore.Reporting;
    using Infrastructure;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using File = System.IO.File;
    using Infrastructure.Models;
    using ApplicationCore.Services.Driver;
    using System.Threading.Tasks;
    using ApplicationCore.Extensions;
    using ApplicationCore.Services.WorkingDay;

    public class ReportService : IReportService
    {
        private const string REPORT_NAME = "..\\Reports\\TimeSheet.rdl";
        
        private readonly IWorkingDayService _workingDayService;
        private readonly IDriverService _driverService;

        public ReportService(
            IWorkingDayService workingDayService, 
            IDriverService driverService)
        {
            this._workingDayService = workingDayService;
            this._driverService = driverService;
        }

        public async Task<byte[]> GenerateTimeSheetReport(TimeSheetReportServiceModel model)
        {
            var reportPath = Path.Combine(Directory.GetCurrentDirectory(), REPORT_NAME);
            if (IsReportExist(reportPath) is false)
            {
                throw new Exception($"Report in directory {reportPath} not found.");
                // TODO: Log the exception
                //return Array.Empty<byte>();
            }

            model.WorkingDays = await this._workingDayService.GetWorkingDaysForFullWeekAsync(model.EmployeeId, model.StartDate);

            var report = new LocalReport(reportPath);
            report.AddDataSource("TimeSheetDataSet", model.WorkingDays);
            
            var result = report.Execute(RenderType.Pdf, 1, await GetTimeSheetReportParameters(model), string.Empty);
            return result.MainStream;
        }

        private async Task<Dictionary<string, string>> GetTimeSheetReportParameters(TimeSheetReportServiceModel model)
            => new ()
            {
                { "WeekStartDate", model.StartDate.FirstDateInWeek().ToString("dd/MM/yyyy") },
                { "EmployeeName", await this._driverService.GetDriverFullNameById(model.EmployeeId) },
                { "TotalHours", $"{this._workingDayService.GetTotalWorkingHours(model.WorkingDays.Select(t => t.TotalHours).ToList()):f2}"},
                { "CompanyName", model.Company },
                { "ManagerName", "Manager Name" },
            };

        #region Helpers

        private static bool IsReportExist(string reportPath) => File.Exists(reportPath);

        #endregion
    }
}
