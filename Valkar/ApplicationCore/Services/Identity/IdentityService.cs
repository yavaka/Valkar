namespace ApplicationCore
{
    using System;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using ApplicationCore.Helpers;
    using ApplicationCore.ServiceModels.Identity;
    using ApplicationCore.Services.Identity;
    using ApplicationCore.Services.Mapper;
    using Infrastructure.Common.Global;
    using Infrastructure.Models;
    using Microsoft.AspNetCore.Identity;

    public class IdentityService : IIdentityService
    {
        private const string INVALID_LOGIN_ERROR = "Invalid email or password.";

        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IMapperService _mapper;

        public IdentityService(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IMapperService mapper)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._mapper = mapper;
        }

        public async Task Register(RegisterServiceModel model)
        {
            var user = this._mapper.Map<RegisterServiceModel, User>(model);
            user.IsCompleted = false;

            var result = await this._userManager
                .CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelErrorHelper.ModelErrors.Add(error.Description);
                }
            }
            else
            {
                await this._userManager.AddToRoleAsync(user, Role.Driver);
            }
        }

        /// <summary>
        /// Login driver
        /// </summary>
        /// <returns>If false the driver details is incomplete</returns>
        public async Task<bool> Login(LoginServiceModel model)
        {
            // Check is user exist
            var user = await GetUserByEmail(model.Email);
            if (user is null)
            {
                ModelErrorHelper.ModelErrors.Add(INVALID_LOGIN_ERROR);
                throw new Exception();
            }

            // Check is the password valid
            var passwordValid = await this._userManager
                .CheckPasswordAsync(user, model.Password);
            if (!passwordValid)
            {
                ModelErrorHelper.ModelErrors.Add(INVALID_LOGIN_ERROR);
                throw new Exception();
            }

            // Sign in user credentials
            await _signInManager.PasswordSignInAsync(
                     user,
                     model.Password,
                     isPersistent: false,
                     lockoutOnFailure: true);

            return user.IsCompleted;
        }

        public async Task Logout()
            => await this._signInManager.SignOutAsync();

        public string GetUserId(ClaimsPrincipal claimsPrincipal)
            => this._userManager.GetUserId(claimsPrincipal);

        public async Task<User> GetUserByEmail(string email)
            => await this._userManager.FindByEmailAsync(email);

        public async Task<string> GeneratePasswordResetToken(User user)
            => await this._userManager.GeneratePasswordResetTokenAsync(user);

        public async Task<IdentityResult> ResetPassword(User user, string token, string newPassword)
            => await this._userManager.ResetPasswordAsync(user, token, newPassword);
        
        public async Task CompleteOnboarding(string userId)
        {
            var user = await this._userManager
                .FindByIdAsync(userId);

            user.IsCompleted = true;

            var result = await this._userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelErrorHelper.ModelErrors.Add(error.Description);
                }
            }
        }

        public async Task<bool> IsOnboardingCompleted(string userId)
        {
            var user = await this._userManager
                .FindByIdAsync(userId);

            if (user.IsCompleted)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> IsAdminLoggedIn(string email) 
        {
            var user = await GetUserByEmail(email);
            var usersInRole = await this._userManager.GetUsersInRoleAsync(Role.Admin);

            if (usersInRole.Any(i =>i.Id == user.Id))
            {
                return true;
            }
            return false;
        }
    }
}