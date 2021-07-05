namespace ApplicationCore
{
    using ApplicationCore.ServiceModels.Identity;
    using ApplicationCore.Services.Identity;
    using Infrastructure.Models;
    using Microsoft.AspNetCore.Identity;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    public class IdentityService : IIdentityService
    {
        private const string INVALID_LOGIN = "Invalid email or password.";

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
            IsUserExists(model.Email, model.UserName);

            var user = new User
            {
                Email = model.Email,
                UserName = model.UserName,
                RegisteredOn = DateTime.Now
            };

            await this._userManager.CreateAsync(user, model.Password);
        }

        public async Task Login(LoginServiceModel model)
        {
            var user = await GetUser(model.Email);
            if (user is null)
            {
                throw new Exception(INVALID_LOGIN);
            }

            var passwordValid = await this._userManager
                .CheckPasswordAsync(user, model.Password);
            if (!passwordValid)
            {
                throw new Exception(INVALID_LOGIN);
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

        private async Task<User> GetUser(string email = default, string userId = default)
        {
            if (email != default)
            {
                return await this._userManager.FindByEmailAsync(email);
            }
            if (userId != default)
            {
                return await this._userManager.FindByIdAsync(userId);
            }
            return null;
        }

        private void IsUserExists(string email, string userName)
        {
            if (this._userManager.Users.Any(e => e.Email == email))
            {
                throw new Exception($"{email} already exist.", new Exception("Email"));
            }
            if (this._userManager.Users.Any(u => u.UserName == userName))
            {
                throw new Exception($"{userName} already exist.", new Exception("UserName"));
            }
        }
    }
}