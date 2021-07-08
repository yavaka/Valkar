namespace ApplicationCore
{
    using System;
    using System.Threading.Tasks;
    using ApplicationCore.Helpers;
    using ApplicationCore.ServiceModels.Identity;
    using ApplicationCore.Services.Identity;
    using Infrastructure.Models;
    using Microsoft.AspNetCore.Identity;

    public class IdentityService : IIdentityService
    {
        private const string INVALID_LOGIN_ERROR = "Invalid email or password.";

        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public IdentityService(
            UserManager<User> userManager,
            SignInManager<User> signInManager)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
        }

        public async Task Register(RegisterServiceModel model)
        {
            var user = new User
            {
                Email = model.Email,
                UserName = model.UserName,
                RegisteredOn = DateTime.Now
            };

            var result = await this._userManager
                .CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelErrorHelper.ModelErrors.Add(error.Description);
                }
            }
        }

        public async Task Login(LoginServiceModel model)
        {
            var user = await GetUser(model.Email);
            if (user is null)
            {
                ModelErrorHelper.ModelErrors.Add(INVALID_LOGIN_ERROR);
            }

            var passwordValid = await this._userManager
                .CheckPasswordAsync(user, model.Password);
            if (!passwordValid)
            {
                ModelErrorHelper.ModelErrors.Add(INVALID_LOGIN_ERROR);
            }

            // Sign in user credentials
            await _signInManager.PasswordSignInAsync(
                     user,
                     model.Password,
                     isPersistent: false,
                     lockoutOnFailure: true);
        }

        public async Task Logout()
            => await this._signInManager.SignOutAsync();

        private async Task<User> GetUser(string email)
            => await this._userManager.FindByEmailAsync(email);
    }
}