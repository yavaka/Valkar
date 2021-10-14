namespace ApplicationCore.Services.WorkingDay
{
    using ApplicationCore.ServiceModels.WorkingDay;
    using ApplicationCore.Services.Mapper;
    using Infrastructure;
    using System.Threading.Tasks;

    public class WorkingDayService : IWorkingDayService
    {
        private readonly ValkarDbContext _data;
        private readonly IMapperService _mapper;

        public WorkingDayService(
            ValkarDbContext data,
            IMapperService mapper)
        {
            this._data = data;
            this._mapper = mapper;
        }

        public async Task AddWorkingDay(WorkingDayServiceModel model)
        {
            var workingDay = this._mapper
                .Map<WorkingDayServiceModel, Infrastructure.Models.WorkingDay>(model);
            
            await this._data.WorkedDays.AddAsync(workingDay);
            await this._data.SaveChangesAsync();
        }
    }
}
