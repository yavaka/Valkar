namespace ApplicationCore.ServiceModels.Identity
{
    using System.ComponentModel.DataAnnotations;
    
    public class ChangePasswordServiceModel
    {
        [Required]
        [Display(Name = "Old Password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [Display(Name = "New Password")]
        public string NewPassword { get; set; }

        [Display(Name = "Confirm password")]
        [Compare(nameof(NewPassword), ErrorMessage = "The password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
