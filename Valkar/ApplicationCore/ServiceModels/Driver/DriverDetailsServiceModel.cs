﻿namespace ApplicationCore.ServiceModels.Driver
{
    using ApplicationCore.Helpers.CheckBox;
    using ApplicationCore.ServiceModels.Document;
    using Infrastructure.Common.Enums;
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using static Infrastructure.Common.ModelConstants;

    public class DriverDetailsServiceModel
    {
        public DriverDetailsServiceModel()
        {
            this.Documents = new EmployeeDocumentsServiceModel();
        }

        public Guid DriverId { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        public Titles Title { get; set; }

        [Required]
        [StringLength(MAX_NAME_LENGTH, ErrorMessage = "{0} must be at least {2} characters long.", MinimumLength = MIN_NAME_LENGTH)]
        [DisplayName("First name/s")]
        public string FirstNames { get; set; }

        [Required]
        [StringLength(MAX_NAME_LENGTH, ErrorMessage = "{0} must be at least {2} characters long.", MinimumLength = MIN_NAME_LENGTH)]
        public string Surname { get; set; }

        [Required]
        [StringLength(MAX_ADDRESS_LENGTH, ErrorMessage = "{0} must be at least {2} characters long.")]
        public string Address { get; set; }

        [Required]
        public string Postcode { get; set; }

        [Required]
        [Phone]
        [DisplayName("Phone number")]
        public string PhoneNumber { get; set; }

        [Required]
        [DisplayName("Date of birth")]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [DisplayName("Driving licence categories")]
        public CheckBoxModel[] DrivingLicenceCategories { get; set; }

        [Required]
        [StringLength(FIXED_NINO_LENGTH, ErrorMessage = "The {0} is {2} characters long.", MinimumLength = FIXED_NINO_LENGTH)]
        [DisplayName("National insurance number")]
        public string NationalInsuranceNumber { get; set; }

        public EmployeeDocumentsServiceModel Documents { get; set; }

        public EmergencyContactServiceModel EmergencyContact { get; set; }

        public LimitedCompanyServiceModel LimitedCompany { get; set; }

        /// <summary>
        /// Radio button value for limited company
        /// </summary>
        public string IsLimitedCompany { get; set; }
    }
}
