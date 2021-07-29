namespace ApplicationCore.ServiceModels.Driver
{
    using ApplicationCore.ServiceModels.Identity;

    public class SettingsServiceModel
    {
        public SettingsServiceModel()
        {
            this.ChangePassword = new ChangePasswordServiceModel();
            this.LimitedCompany = new LimitedCompanyServiceModel();
        }

        public UpdateDriverDetailsServiceModel DriverDetails { get; set; }

        public ChangePasswordServiceModel ChangePassword { get; set; }

        public LimitedCompanyServiceModel LimitedCompany { get; set; }
    }
}
