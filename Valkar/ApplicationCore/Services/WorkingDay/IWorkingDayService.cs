namespace ApplicationCore.Services.WorkingDay
{
    using ApplicationCore.ServiceModels.WorkingDay;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IWorkingDayService
    {
        Task AddWorkingDay(WorkingDayServiceModel model);
        Task EditWorkingDay(WorkingDayServiceModel model);
        Task DeleteWorkingDay(int id);
        Task<WorkingDayServiceModel> GetWorkingDayById(int id);
        Task<ICollection<WorkingDayServiceModel>> GetWorkingDaysByDriverId(string driverId);
        /// <summary>
        /// Get working days for a full week. For days that there is no records add an empty working day (day off)
        /// </summary>
        /// <param name="driverId"></param>
        /// <param name="weekStart"></param>
        /// <returns></returns>
        Task<ICollection<WorkingDayServiceModel>> GetWorkingDaysForFullWeekAsync(string driverId, DateTime weekStart);
        double GetTotalWorkingHours(List<TimeSpan> workingDaysTotalHours);
    }
}
