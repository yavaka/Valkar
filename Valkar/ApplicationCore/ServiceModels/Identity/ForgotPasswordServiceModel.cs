namespace ApplicationCore.ServiceModels.Identity
{
    using System.ComponentModel.DataAnnotations;

    public class ForgotPasswordServiceModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
