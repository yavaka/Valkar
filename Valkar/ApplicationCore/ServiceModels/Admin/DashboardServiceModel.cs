namespace ApplicationCore.ServiceModels.Admin
{
    using System.Collections.Generic;

    public class DashboardServiceModel
    {
        public DashboardServiceModel()
            => this.Drivers = new List<DriverServiceModel>();

        public IEnumerable<DriverServiceModel> Drivers { get; set; }
    }
}
