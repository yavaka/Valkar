namespace ApplicationCore.ServiceModels.Driver
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using static Infrastructure.Common.ModelConstants;

    public class LimitedCompanyServiceModel
    {
        public string CompanyName { get; set; }
        [StringLength(FIXED_NINO_LENGTH, ErrorMessage = "The {0} is {2} digits.", MinimumLength = FIXED_NINO_LENGTH)]
        [DisplayName("Company registration number")]
        public string CompanyRegistrationNumber { get; set; }
    }
}
