using System.ComponentModel.DataAnnotations;

namespace ApplicationCore.ServiceModels.Identity
{
    public class LoginServiceModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
        
        [Required]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }
}