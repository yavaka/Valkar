namespace Infrastructure.Models
{
    using Infrastructure.Common.Enums;
    using System;

    public class LicenceCategory
    {
        public int Id { get; set; }
        public DrivingLicenceCategories Category{ get; set; }

        public Guid DriverId { get; set; }
        public Driver Driver { get; set; }
    }
}
