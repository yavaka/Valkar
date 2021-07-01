namespace Valkar.Application.ViewModels.Identity
{
    public class LoginViewModel
    {
        public LoginViewModel()
        {
            this.Email = default!;
            this.Password = default!;
        }

        public string Email { get; set; }
        public string Password { get; set; }
    }
}
