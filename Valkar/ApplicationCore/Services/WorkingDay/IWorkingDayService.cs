namespace ApplicationCore.Services.WorkingDay
{
    using ApplicationCore.ServiceModels.WorkingDay;
    using System.Threading.Tasks;

    public interface IWorkingDayService
    {
        Task AddWorkingDay(WorkingDayServiceModel model);
    }
}
