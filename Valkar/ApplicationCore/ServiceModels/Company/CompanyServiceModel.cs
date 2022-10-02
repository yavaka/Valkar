namespace ApplicationCore.ServiceModels.Company
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using static Infrastructure.Common.ModelConstants;

    public class CompanyServiceModel
    {
        public string Id { get; set; }

        [Required]
        [StringLength(MAX_COMPANY_NAME_LENGTH, ErrorMessage = "The {0} is {2} digits.", MinimumLength = MIN_COMPANY_NAME_LENGTH)]
        [DisplayName("Company name")]
        public string Name { get; set; }

        [Required]
        [Phone]
        [DisplayName("Phone number")]
        public string PhoneNumber { get; set; }

        [Required]
        [EmailAddress]
        [DisplayName("Email address")]
        public string EmailAddress { get; set; }

        [DisplayName("Registration Number")]
        public string RegistrationNumber { get; set; }

        [Required]
        [StringLength(MAX_ADDRESS_LENGTH, ErrorMessage = "{0} must be at least {2} characters long.")]
        [DisplayName("Office Address")]
        public string OfficeAddress { get; set; }

        [Required]
        [DisplayName("Office Postcode")]
        public string OfficePostCode { get; set; }

    }
}
