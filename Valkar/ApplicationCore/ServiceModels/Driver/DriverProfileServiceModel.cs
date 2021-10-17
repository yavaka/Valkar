namespace ApplicationCore.ServiceModels.Driver
{
    using ApplicationCore.ServiceModels.WorkingDay;
    using System.Collections.Generic;

    public class DriverProfileServiceModel
    {
        public DriverProfileServiceModel()
            => this.WorkedDays = new List<WorkingDayServiceModel>();

        public string DriverId { get; set; }
        public string Title { get; set; }
        public string Surname { get; set; }
        public ICollection<WorkingDayServiceModel> WorkedDays { get; set; }
    }
}
