namespace Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using ApplicationCore.Helpers;
    using ApplicationCore.ServiceModels.Identity;
    using ApplicationCore.Services.Email;
    using ApplicationCore.Services.Identity;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    public class IdentityController : Controller
    {
        private readonly IIdentityService _identityService;
        private readonly IEmailService _emailService;

        public IdentityController(
            IIdentityService identityService,
            IEmailService emailService)
        {
            this._identityService = identityService;
            this._emailService = emailService;
        }

        [HttpGet]
        public IActionResult Register()
            => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterServiceModel model)
        {
            if (ModelState.IsValid)
            {
                // Register - identity serivce
                await this._identityService.Register(model);

                // Success
                if (ModelErrorHelper.ModelErrors.Count is 0)
                {
                    return RedirectToAction("Login");
                }

                // Errors
                foreach (var error in ModelErrorHelper.ModelErrors)
                {
                    ModelState.AddModelError(string.Empty, error);
                }
                ModelErrorHelper.ModelErrors = new List<string>();

                return View(model);
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
            => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginServiceModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Whether or not the driver details completed
                    var isCompleted = await this._identityService.Login(model);

                    // Redirect to driver profile
                    if (isCompleted)
                    {
                        return RedirectToAction("Profile", "Drivers");
                    }
                    // Redirect to driver details form
                    return RedirectToAction("DriverDetails", "Drivers");
                }
                catch (Exception e)
                {
                    // Add model error from sign in manager
                    if (ModelErrorHelper.ModelErrors.Count is 0)
                    {
                        ModelState.AddModelError("Login", e.Message);
                    }
                    // Add model error from identity service login method
                    else
                    {
                        foreach (var error in ModelErrorHelper.ModelErrors)
                        {
                            ModelState.AddModelError(string.Empty, error);
                        }
                        ModelErrorHelper.ModelErrors = new List<string>();
                    }
                }
            }
            return View(model);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await this._identityService.Logout();

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordServiceModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await this._identityService
                    .GetUserByEmail(model.Email);
                if (user is null)
                {
                    TempData["invalidUser"] = $"Account with email {model.Email} cannot be found.";
                    return View(model);
                }

                // Generate password reset token
                var token = await this._identityService
                    .GeneratePasswordResetToken(user);

                // Compose callback url
                var callback = Url.Action(
                    action: nameof(ResetPassword),
                    controller: "Identity",
                    values: new
                    {
                        token,
                        Email = user.Email
                    },
                    protocol: Request.Scheme);

                // Send email
                await this._emailService.SendEmail(new Message(
                    to: new string[] { user.Email },
                    subject: "Valkar account reset password",
                    content: $"Reset password link: \n\r{callback}"));

                TempData["sentEmail"] = "We have sent an email with password reset link. Please check your mailbox.";
                return View();
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult ResetPassword(string token, string email)
        {
            var model = new ResetPasswordServiceModel { Token = token, Email = email };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordServiceModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await this._identityService.GetUserByEmail(model.Email);
                if (user == null)
                {
                    TempData["invalidUser"] = $"Account with email {model.Email} cannot be found.";
                    return RedirectToAction(nameof(ForgotPassword));
                }

                var resetPassResult = await this._identityService
                    .ResetPassword(user, model.Token, model.Password);
                if (!resetPassResult.Succeeded)
                {
                    foreach (var error in resetPassResult.Errors)
                    {
                        ModelState.TryAddModelError(nameof(ResetPasswordServiceModel.Password), error.Description);
                    }

                    return View();
                }

                TempData["passwordChanged"] = "Your password was changed successfully. Now you can login with your new password.";
                return RedirectToAction(nameof(Login));
            }
            return View(model);
        }
    }
}
