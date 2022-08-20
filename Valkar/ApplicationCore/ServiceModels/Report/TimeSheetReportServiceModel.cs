namespace ApplicationCore.ServiceModels.Report
{
    using ApplicationCore.ServiceModels.WorkingDay;
    using System;
    using System.Collections.Generic;

    public class TimeSheetReportServiceModel : ServiceModel
    {
        public string EmployeeId { get; set; }

        private DateTime _startDate = DateTime.Today;
        public DateTime StartDate
        {
            get => this._startDate.Date;
            set => this._startDate = value;
        }

        public string Company { get; set; }
        public string ManagerName { get; set; }

        public IEnumerable<WorkingDayServiceModel> WorkingDays { get; set; }
    }
}
