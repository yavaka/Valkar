namespace ApplicationCore.ServiceModels.Document
{
    using Microsoft.AspNetCore.Http;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    public class UploadSignedDocumentServiceModel
    {
        [Required]
        [DisplayName("Upload document")]
        public IFormFile File { get; set; }
        
        public TempDocumentServiceModel Document { get; set; }
    }
}
