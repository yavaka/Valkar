namespace ApplicationCore.ServiceModels.Driver
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using static Infrastructure.Common.ModelConstants;

    public class LimitedCompanyServiceModel
    {
        public LimitedCompanyServiceModel()
        {
            this.CompanyName = "";
            this.CompanyRegistrationNumber = "";
        }

        [StringLength(MAX_COMPANY_NAME_LENGTH, ErrorMessage = "The {0} is {2} digits.", MinimumLength = MIN_COMPANY_NAME_LENGTH)]
        [DisplayName("Company name")]
        public string CompanyName { get; set; }
        [StringLength(FIXED_COMPANY_REGISTRATION_NUMBER, ErrorMessage = "The {0} is {2} digits.", MinimumLength = FIXED_COMPANY_REGISTRATION_NUMBER)]
        [DisplayName("Company registration number")]
        public string CompanyRegistrationNumber { get; set; }
    }
}
