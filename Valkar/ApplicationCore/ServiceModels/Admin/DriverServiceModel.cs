namespace ApplicationCore.ServiceModels.Admin
{
    public class DriverServiceModel
    {
        /// <summary>
        /// The account id of the driver
        /// </summary>
        public string UserId { get; set; }
        public string DriverId { get; set; }
        public string FirstNames { get; set; }
        public string Surname { get; set; }
        public string FullName => $"{this.FirstNames.Split(' ')[0]} {this.Surname}";
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }
}
