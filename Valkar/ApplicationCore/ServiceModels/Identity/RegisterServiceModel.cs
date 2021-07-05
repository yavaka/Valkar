namespace ApplicationCore.ServiceModels.Identity
{
    using System.ComponentModel.DataAnnotations;
    
    public class RegisterServiceModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(10, ErrorMessage = "The {0} must be at least {2} and max {1} characters long.", MinimumLength = 6)]
        [Display(Name = "UserName")]
        public string UserName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and max {1} characters long.", MinimumLength = 6)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
