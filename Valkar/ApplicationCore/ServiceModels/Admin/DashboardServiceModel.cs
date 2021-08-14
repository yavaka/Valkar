namespace ApplicationCore.ServiceModels.Admin
{
    using System.Collections.Generic;

    public class DashboardServiceModel
    {
        public DashboardServiceModel()
            => this.Drivers = new List<DriverAdminServiceModel>();

        public IEnumerable<DriverAdminServiceModel> Drivers { get; set; }
    }
}
