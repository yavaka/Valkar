﻿namespace ApplicationCore.ServiceModels.Driver
{
    using Infrastructure.Common.Enums;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using static Infrastructure.Common.ModelConstants;

    public class EmergencyContactServiceModel
    {
        [Required(ErrorMessage = "Title is required.")]
        public Titles Title { get; set; }
        [Required]
        [StringLength(MAX_NAME_LENGTH, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = MIN_NAME_LENGTH)]
        [DisplayName("First name/s")]
        public string FirstNames { get; set; }
        [Required]
        [StringLength(MAX_NAME_LENGTH, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = MIN_NAME_LENGTH)]
        public string Surname { get; set; }
        [Required]
        [StringLength(MAX_ADDRESS_LENGTH, ErrorMessage = "The {0} must be at least {2} characters long.")]
        public string Address { get; set; }
        [Required]
        public string Postcode { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [Phone]
        [DisplayName("Phone number")]
        public string PhoneNumber { get; set; }
        [Required]
        public string Relationship { get; set; }
    }
}
