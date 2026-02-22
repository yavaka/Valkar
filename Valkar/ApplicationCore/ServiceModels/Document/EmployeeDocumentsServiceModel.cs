namespace ApplicationCore.ServiceModels.Document
{
    using Microsoft.AspNetCore.Http;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    public class EmployeeDocumentsServiceModel
    {
        [Required]
        [DisplayName("Driving Licence (Front)")]
        public IFormFile DrivingLicenceFront { get; set; }

        [Required]
        [DisplayName("Driving Licence (Back)")]
        public IFormFile DrivingLicenceBack { get; set; }

        [Required]
        [DisplayName("Identity Document (Front)")]
        public IFormFile IdentityDocumentFront { get; set; }

        [DisplayName("Identity Document (Back)")]
        public IFormFile IdentityDocumentBack { get; set; }

        [Required]
        [DisplayName("National Insurance Number (Letter)")]
        public IFormFile NationalInsuranceNumber { get; set; }
    }
}
