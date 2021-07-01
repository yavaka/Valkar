namespace Valkar.Application.ViewModels.Identity
{
    public class RegisterViewModel
    {
        public RegisterViewModel()
        {
            this.Email = default!;
            this.UserName = default!;
            this.Password = default!;
        }

        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
