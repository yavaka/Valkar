namespace ApplicationCore.Services.WorkingDay
{
    using ApplicationCore.ServiceModels.WorkingDay;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IWorkingDayService
    {
        Task AddWorkingDay(WorkingDayServiceModel model);
        Task EditWorkingDay(WorkingDayServiceModel model);
        Task DeleteWorkingDay(int id);
        Task<WorkingDayServiceModel> GetWorkingDayById(int id);
        Task<ICollection<WorkingDayServiceModel>> GetWorkingDaysByDriverId(string driverId);
    }
}
