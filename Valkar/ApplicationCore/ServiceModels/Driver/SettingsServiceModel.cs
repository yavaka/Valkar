namespace ApplicationCore.ServiceModels.Driver
{
    using ApplicationCore.ServiceModels.Identity;

    public class SettingsServiceModel
    {
        public UpdateDriverDetailsServiceModel DriverDetails { get; set; }

        public ChangePasswordServiceModel ChangePassword { get; set; }

        public LimitedCompanyServiceModel LimitedCompany { get; set; }
    }
}
