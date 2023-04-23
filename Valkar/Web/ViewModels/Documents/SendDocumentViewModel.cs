namespace Web.ViewModels.Documents
{
    using Microsoft.AspNetCore.Http;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class SendDocumentViewModel
    {
        [Required]
        [DisplayName("Upload document")]
        public IFormFile File { get; set; }

        public string EmployeeId { get; set; }

        [Required]
        [DisplayName("Message")]
        public string MessageToEmployee { get; set; }

        [DisplayName("Send to")]
        public SelectList EmployeeNameSelectList { get; set; }
    }
}
