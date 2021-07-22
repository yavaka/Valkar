namespace ApplicationCore.ServiceModels.Identity
{
    using System.ComponentModel.DataAnnotations;

    public class ResetPasswordServiceModel
    {
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Email { get; set; }

        public string Token { get; set; }
    }
}
