﻿namespace ApplicationCore.ServiceModels.Document
{
    using Microsoft.AspNetCore.Http;
    using System.ComponentModel;

    public class EmployeeDocumentsServiceModel
    {
        [DisplayName("Driving Licence (Front)")]
        public IFormFile DrivingLicenceFront { get; set; }

        [DisplayName("Driving Licence (Back)")]
        public IFormFile DrivingLicenceBack { get; set; }

        [DisplayName("Identity Document (Front)")]
        public IFormFile IdentityDocumentFront { get; set; }

        [DisplayName("Identity Document (Back)")]
        public IFormFile IdentityDocumentBack { get; set; }

        [DisplayName("National Insurance Number (Letter)")]
        public IFormFile NationalInsuranceNumber { get; set; }
    }
}
