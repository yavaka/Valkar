namespace Valkar.Infrastructure.Identity
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Valkar.Application.Interfaces.Identity;
    using Valkar.Application.ViewModels.Identity;

    internal class IdentityService : IIdentity
    {
        private readonly ILogger<IdentityService> _logger;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public IdentityService(
            ILogger<IdentityService> logger,
            UserManager<User> userManager,
            SignInManager<User> signInManager)
        {
            this._logger = logger;
            this._userManager = userManager;
            this._signInManager = signInManager;
        }

        public async Task<bool> Register(RegisterViewModel model)
        {
            var user = new User
            {
                Email = model.Email,
                UserName = model.UserName,
                RegisterOn = DateTime.Now
            };

            // Create new user
            var identityResult = await this._userManager.CreateAsync(user, model.Password);
            // Grab errors if there are so
            var errors = string.Join(',', identityResult.Errors.Select(e => e.Description));

            // Log success or list errors
            if (identityResult.Succeeded)
            {
                this._logger.LogInformation($"User, {user.UserName} was registered");
                return true;
            }
            else
            {
                this._logger.LogError(errors);
                return false;
            }
        }

        public async Task<bool> Login(LoginViewModel model)
        {
            // User validation
            var user = await this._userManager
                .FindByEmailAsync(model.Email);
            if (user is null)
            {
                this._logger
                    .LogError($"Account with email: {model.Email} cannot be found.");
                return false;
            }

            // Password validation
            var passwordValid = await this._userManager
                .CheckPasswordAsync(user, model.Password);
            if (!passwordValid)
            {
                this._logger
                    .LogError("Invalid password!");
                return false;
            }

            // Sign in user credentials
            var result = await _signInManager
                .PasswordSignInAsync(
                    user,
                    model.Password,
                    isPersistent: false,
                    lockoutOnFailure: true);

            // Successfuly logged in
            if (result.Succeeded)
            {
                this._logger.LogInformation($"User: {user.Email} has logged in.");
                return true;
            }
            // Login failed
            this._logger.LogError($"Login failed.");
            return false;
        }

        public IEnumerable<IUser> GetUsers()
            => this._userManager.Users.ToList();
    }
}
