namespace ApplicationCore.Services.WorkingDay
{
    using ApplicationCore.ServiceModels.WorkingDay;
    using ApplicationCore.Services.Mapper;
    using Infrastructure;
    using Microsoft.EntityFrameworkCore;
    using Infrastructure.Models;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using System.Linq;

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
                .Map<WorkingDayServiceModel, WorkingDay>(model);
            
            await this._data.WorkedDays.AddAsync(workingDay);
            await this._data.SaveChangesAsync();
        }

        public async Task EditWorkingDay(WorkingDayServiceModel model)
        {
            this._data.WorkedDays.Update(this._mapper
                .Map<WorkingDayServiceModel, WorkingDay>(model));
            await this._data.SaveChangesAsync();
        }

        public async Task DeleteWorkingDay(int id)
        {
            this._data.WorkedDays.Remove(await GetWorkingDayModelById(id));
            await this._data.SaveChangesAsync();
        }

        public async Task<WorkingDayServiceModel> GetWorkingDayById(int id)
            => this._mapper.Map<WorkingDay, WorkingDayServiceModel>(await GetWorkingDayModelById(id));

        private async Task<WorkingDay> GetWorkingDayModelById(int id)
            => await this._data.WorkedDays.FirstOrDefaultAsync(i => i.Id == id);

        public async Task<ICollection<WorkingDayServiceModel>> GetWorkingDaysByDriverId(string driverId)
            => await this._data.WorkedDays
                .Where(i => i.DriverId.ToString() == driverId)
                .Select(wd => 
                    this._mapper.Map<WorkingDay, WorkingDayServiceModel>(wd))
                .ToListAsync();
    }
}
